using System.Text;

using MathNet.Numerics.LinearAlgebra.Double;

using OperationsLogic.Algorithms;

namespace OperationsLogic.Model;
public class CanonicalTableau
{
    public int Rows { get; set; }
    public int DecisionVars { get; set; }
    public int ExcessVars { get; set; }
    public int SlackVars { get; set; }
    public int TotalVars => DecisionVars + ExcessVars + SlackVars; // Column count
    public double[,] Tableau { get; private set; } // Matrix style table
    public double[,] NonCanonicalTableau { get; private set; } // Required for functions such as add constraint that perform math preliminaries and stored seperatly in-case Tableau is optimized and overwritten
    public bool IsMaximization { get; set; }
    public string[] SignRestrictions { get; }  // specify +, -, urs, int or bin for each decision var in inputLines

    private static readonly string[] ValidOperatorsAndTypes = ["+", "-", "urs", "int", "bin"];
    private static readonly string[] ValidConstraintOperators = ["<=", ">=", "="];
    public List<int> BasicVariableIndices { get; private set; } = [];

    public CanonicalTableau() { }

    public CanonicalTableau(string[] inputLines)
    {
        if (inputLines == null || inputLines.Length < 2)
            throw new ArgumentException("Input must have at least an objective and one constraint.");

        /* Empty table setup */

        string[] objParts = inputLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Example
        // objParts[0]: max
        // objParts[1]: +3
        // objParts[2]: + 2

        IsMaximization = objParts[0].Equals("max", StringComparison.CurrentCultureIgnoreCase);
        DecisionVars = objParts.Length - 1; // objPart[0] is max or min. Remove it

        double[] objCoefficients = new double[DecisionVars];
        for (int i = 0; i < DecisionVars; i++)
        {
            string term = objParts[i + 1]; // e.g +3

            if (string.IsNullOrEmpty(term) || (term[0] != '+' && term[0] != '-'))
                throw new ArgumentException($"Invalid operator in objective: {term}");

            // The .. is a range operator that means starting at index x, get the rest of the values to the end.
            if (!double.TryParse(term[1..], out double coeff))
                throw new ArgumentException($"Invalid coefficient in objective: {term}");

            objCoefficients[i] = term[0] == '+' ? coeff : -coeff;
        }

        // [.. is a simplified form of .ToArray
        string[] constraints = [.. inputLines.Skip(1).TakeWhile(line => !line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
             .All(termPart => ValidOperatorsAndTypes.Contains(termPart)))];

        // e.g
        // +2 + 1 <= 100
        // + 1 + 1 <= 80
        // + 1 <= 40

        Rows = constraints.Length + 1;  // Objective function with constraints
        SlackVars = constraints.Count(line => line.Contains("<="));
        ExcessVars = constraints.Count(line => line.Contains(">="));

        Tableau = new double[Rows, TotalVars + 1];
        NonCanonicalTableau = new double[Rows, TotalVars + 1];

        /* Enter values */
        for (int decisionVar = 0; decisionVar < DecisionVars; decisionVar++)
            Tableau[0, decisionVar] = IsMaximization ? -objCoefficients[decisionVar] : objCoefficients[decisionVar];
        Tableau[0, TotalVars] = 0; // RHS is always 0 for Obj Func

        for (int decisionVar = 0; decisionVar < DecisionVars; decisionVar++)
            NonCanonicalTableau[0, decisionVar] = objCoefficients[decisionVar];
        NonCanonicalTableau[0, TotalVars] = 0;

        int slackIndex = 0, excessIndex = 0;

        for (int row = 0; row < constraints.Length; row++)
        {
            string[] parts = constraints[row].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                throw new ArgumentException($"Constraint {row + 1} has invalid number of terms.");
            if (!ValidConstraintOperators.Contains(parts[^2]))
                throw new ArgumentException($"Invalid relation in constraint {row + 1}: {parts[^2]}");

            // Decision variable 
            int coeffCount = parts.Length - 2;  // Coefficients before relation, RHS
            if (coeffCount > DecisionVars)
                throw new ArgumentException($"Constraint {row + 1} has too many coefficients.");
            for (int col = 0; col < DecisionVars; col++)
            {
                if (col < coeffCount)
                {
                    string term = parts[col];  // e.g., +2
                    if (string.IsNullOrEmpty(term) || (term[0] != '+' && term[0] != '-'))
                        throw new ArgumentException($"Invalid operator in constraint {row + 1}: {term}");
                    if (!double.TryParse(term[1..], out double coeff))
                        throw new ArgumentException($"Invalid coefficient in constraint {row + 1}: {term}");
                    Tableau[row + 1, col] = parts[^2] == ">=" ? -(term[0] == '+' ? coeff : -coeff) : term[0] == '+' ? coeff : -coeff;
                    NonCanonicalTableau[row + 1, col] = coeff;
                }
                else
                {
                    Tableau[row + 1, col] = 0;  // Implied 0 for missing coefficients
                    NonCanonicalTableau[row + 1, col] = 0;
                }
            }

            // RHS
            if (!double.TryParse(parts[^1], out double rhs)) // ^1 means count from last decending
                throw new ArgumentException($"Invalid RHS in constraint {row + 1}: {parts[^1]}");
            Tableau[row + 1, TotalVars] = parts[^2] == ">=" ? -rhs : rhs;
            NonCanonicalTableau[row + 1, TotalVars] = rhs;

            // Slack/excess variables
            string relation = parts[^2];
            if (relation == "<=")
            {
                Tableau[row + 1, DecisionVars + ExcessVars + slackIndex] = 1;  // Slack variable
                NonCanonicalTableau[row + 1, DecisionVars + ExcessVars + slackIndex] = 1;
                slackIndex++;
            }
            else if (relation == ">=")
            {
                Tableau[row + 1, DecisionVars + excessIndex] = 1;  // Excess variable
                NonCanonicalTableau[row + 1, DecisionVars + excessIndex] = -1;
                excessIndex++;
            }
            else if (relation == "=")
            {
                // Unsure if row did URS correctly. Need to test
                Tableau[row + 1, DecisionVars + ExcessVars + slackIndex] = 1;  // Slack variable
                Tableau[row + 1, DecisionVars + excessIndex] = 1;  // Excess variable

                slackIndex++;
                excessIndex++;
            }
            else
            {
                throw new ArgumentException($"Unknown relation in constraint {row + 1}: {relation}");
            }
        }

        // Sign restrictions
        SignRestrictions = inputLines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (SignRestrictions.Length != DecisionVars)
            throw new ArgumentException("Sign restrictions must match number of decision variables.");
        foreach (string? restriction in SignRestrictions)
            if (!ValidOperatorsAndTypes.Contains(restriction))
                throw new ArgumentException($"Invalid sign restriction: {restriction}");

        // Validate tableau
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < TotalVars + 1; col++)
            {
                if (double.IsNaN(Tableau[row, col]) || double.IsInfinity(Tableau[row, col]))
                    throw new ArgumentException($"Invalid value in Tableau at [{row},{col}]: NaN or Infinity.");
                if (double.IsNaN(NonCanonicalTableau[row, col]) || double.IsInfinity(NonCanonicalTableau[row, col]))
                    throw new ArgumentException($"Invalid value in NonCanonicalTableau at [{row},{col}]: NaN or Infinity.");
            }
        }
    }

    public CanonicalTableau(int decisionVars, int excessVars, int slackVars, bool isMaximization, string[] signRestrictions, double[,] tableau, double[,] noncanonicaltableau)
    {
        DecisionVars = decisionVars;
        ExcessVars = excessVars;
        SlackVars = slackVars;
        Rows = tableau.GetLength(0);
        Tableau = tableau;
        NonCanonicalTableau = noncanonicaltableau;
        IsMaximization = isMaximization;
        SignRestrictions = signRestrictions;
    }


    #region Tableau manipulation
    public static (bool, string model) IsOptimal(CanonicalTableau tableau)
    {
        // Check objective row for negative values. If obj negative, perform Primal Simplex
        for (int col = 0; col < tableau.TotalVars; col++)
        {
            if (tableau.Tableau[0, col] < 0)
                return (false, "Primal Simplex");
        }

        // Check RHS column for negative values. If RHS negative, perform Dual Simplex
        double[] rhsColumn = tableau.GetColumn(tableau.TotalVars);
        if (rhsColumn.Any(value => value < 0))
            return (false, "Dual Simplex");

        return (true, "");
    }

    public string DisplayTableau(double[]? thetas = null, bool displayNonCanonical = false)
    {
        StringBuilder sb = new();

        _ = sb.Append("T\t");
        for (int i = 0; i < DecisionVars; i++)
            _ = sb.Append($"x{i + 1}\t");
        for (int i = 0; i < ExcessVars; i++)
            _ = sb.Append($"E{i + 1}\t");
        for (int i = 0; i < SlackVars; i++)
            _ = sb.Append($"S{i + 1}\t");
        _ = sb.Append("RHS\n");

        double[,] displayedTableau = displayNonCanonical ? NonCanonicalTableau : Tableau;

        for (int row = 0; row < Rows; row++)
        {
            _ = sb.Append(row == 0 ? "Z\t" : $"{row}\t");
            for (int col = 0; col < TotalVars + 1; col++)
                _ = sb.Append($"{Math.Round(displayedTableau[row, col], 3)}\t");
            _ = sb.Append('\n');
        }

        if (thetas != null)
        {
            _ = sb.Append("Theta\t");
            for (int col = 0; col < TotalVars; col++)
                _ = sb.Append($"{Math.Round(thetas[col], 3)}\t");
            _ = sb.AppendLine();
        }

        return sb.ToString();
    }

    public CanonicalTableau PerformPivot(int pivotRow, int pivotColumn)
    {
        if (pivotRow < 0 || pivotRow >= Rows || pivotColumn < 0 || pivotColumn >= TotalVars)
            throw new ArgumentException("Invalid pivot index provided");

        double pivotValue = Tableau[pivotRow, pivotColumn];
        if (pivotValue == 0)
            throw new InvalidOperationException("Pivot value cannot be zero as you cannot divide by zero");

        double[,] newTableau = new double[Rows, TotalVars + 1];
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < TotalVars + 1; col++)
            {
                newTableau[row, col] = row == pivotRow ? Tableau[row, col] / pivotValue : Tableau[row, col] - (Tableau[row, pivotColumn] * Tableau[pivotRow, col] / pivotValue);
                newTableau[row, col] = newTableau[row, col] == 0 ? 0 : Math.Round(newTableau[row, col], 3);
            }
        }

        return new CanonicalTableau(DecisionVars, ExcessVars, SlackVars, IsMaximization, SignRestrictions, newTableau, NonCanonicalTableau);
    }

    // TODO: Using CanonicalTableau 3 times is inefficient. Come back to improve if there is time
    public CanonicalTableau AddConstraint(double[] coefficients, string relation, double rhs)
    {
        if (coefficients.Length == 0)
            throw new ArgumentException("Must have atleast one coefficient to add constraint.");
        if (coefficients.Length > DecisionVars)
            throw new ArgumentException($"Number of coefficients ({coefficients.Length}) exceeds number of decision variables ({DecisionVars}).");
        if (!ValidConstraintOperators.Contains(relation))
            throw new ArgumentException($"Invalid relation: {relation}. Must be one of the following relations - {string.Join(", ", ValidConstraintOperators)}.");
        if (double.IsNaN(rhs) || double.IsInfinity(rhs))
            throw new ArgumentException($"Invalid RHS value: {rhs}. Must be a finite number.");

        foreach (double coeff in coefficients)
            if (double.IsNaN(coeff) || double.IsInfinity(coeff))
                throw new ArgumentException($"Invalid coefficient value: {coeff}. Must be a finite number.");

        // Get list of xBVIndices prior to changes. This is to detect conflicts
        List<int> basicVariablesIndices = [];
        for (int col = 0; col < DecisionVars; col++)
        {
            if (IsBasicVariable(col))
            {
                basicVariablesIndices.Add(col);
            }
        }

        int newSlackVars = SlackVars + (relation == "<=" ? 1 : 0);
        int newExcessVars = ExcessVars + (relation == ">=" ? 1 : 0);
        if (relation == "=")
        {
            newSlackVars++;
            newExcessVars++;
        }
        int newTotalVars = DecisionVars + newExcessVars + newSlackVars;
        int newRows = Rows + 1; // Only one constraint added at a time with this function. Multiple can be added via Loop

        double[,] newTableau = new double[newRows, newTotalVars + 1];

        // Copy original tables contents to new table
        for (int row = 0; row < Rows; row++)
            for (int col = 0; col < TotalVars + 1; col++)
                if (col == TotalVars)
                {
                    newTableau[row, newTotalVars] = Tableau[row, col];
                }
                else
                {
                    newTableau[row, col] = Tableau[row, col];
                }

        // Set new row contents
        int newRow = Rows;
        for (int col = 0; col < DecisionVars; col++)
        {
            newTableau[newRow, col] = col < coefficients.Length ? (relation == ">=" ? -coefficients[col] : coefficients[col]) : 0; // Implied 0 for missing coefficients
        }
        newTableau[newRow, newTotalVars] = relation == ">=" ? -rhs : rhs;

        if (relation == "<=")
        {
            newTableau[newRow, DecisionVars + ExcessVars + SlackVars] = 1;
        }
        else if (relation == ">=")
        {
            newTableau[newRow, DecisionVars + ExcessVars] = 1;
        }
        else if (relation == "=")
        {
            newTableau[newRow, DecisionVars + ExcessVars + SlackVars] = 1;
            newTableau[newRow, DecisionVars + ExcessVars] = 1;
        }

        for (int row = 0; row < newRows; row++)
            for (int col = 0; col < newTotalVars + 1; col++)
                if (double.IsNaN(newTableau[row, col]) || double.IsInfinity(newTableau[row, col]))
                    throw new ArgumentException($"Invalid value at [{row},{col}]: NaN or Infinity.");

        CanonicalTableau conflictTableau = new(DecisionVars, newExcessVars, newSlackVars, IsMaximization, SignRestrictions, newTableau, NonCanonicalTableau);

        CanonicalTableau? resultTableau = null;
        foreach (int basicVariableIndex in basicVariablesIndices)
        {
            bool previouslyBasicRemainsBasic = conflictTableau.IsBasicVariable(basicVariableIndex);
            if (previouslyBasicRemainsBasic != true)
            {
                // Need to resolve conflict between two 1's in single column, so it may be basic again
                double[] column = conflictTableau.GetColumn(basicVariableIndex);

                List<int> oneRows = [];
                for (int row = 0; row < newRows; row++)
                {
                    double value = column[row];
                    if (value == 1)
                    {
                        oneRows.Add(row);
                    }
                }

                if (oneRows.Count >= 2)
                {
                    // Subtract new row from the original basic row
                    int originalRow = oneRows.First(row => row != newRow);

                    for (int col = 0; col < newTotalVars + 1; col++)
                    {
                        newTableau[newRows - 1, col] = newTableau[originalRow, col] - newTableau[newRows - 1, col];
                    }

                    // If RHS is positive, multiply row by -1 for dual simplex
                    if (newTableau[newRows - 1, newTotalVars] > 0)
                    {
                        for (int col = 0; col < newTotalVars + 1; col++)
                        {
                            newTableau[newRows - 1, col] = newTableau[newRows - 1, col] * -1 == 0 ? 0 : newTableau[newRows - 1, col] * -1;
                        }
                    }

                    if (!IsBasicVariable(basicVariableIndex))
                        throw new InvalidOperationException($"Failed to restore basic variable at column {basicVariableIndex}.");

                    conflictTableau = new(DecisionVars, newExcessVars, newSlackVars, IsMaximization, SignRestrictions, newTableau, NonCanonicalTableau);

                    (_, resultTableau) = DualSimplex.Solve(conflictTableau);
                }
                else
                {
                    throw new InvalidOperationException("There should always be at least 2 values of one conflicting when adding a constraint");
                }

            }
        }

        // Whatever uses this function needs to check that the row count has increased for success
        return resultTableau ?? this;
    }

    // TODO: Similar to AddConstraint, using CanonicalTableau is likely inefficient. Come back to improve if there is time
    public CanonicalTableau AddActivity(double[] newColumnValues)
    {
        if (newColumnValues.Length != Rows)
            throw new ArgumentException($"The number of column values must match the number of rows ({Rows}).");

        int newDecisionVars = DecisionVars + 1;
        int newTotalVars = TotalVars + 1;
        double[,] newNonCanonicalTableau = new double[Rows, newTotalVars + 1];
        double[,] newTableau = new double[Rows, newTotalVars + 1];

        for (int row = 0; row < Rows; row++)
            for (int col = 0; col < newTotalVars + 1; col++)
            {
                // Copy old vars, insert new column, shift existing right
                if (col < DecisionVars)
                {
                    newNonCanonicalTableau[row, col] = NonCanonicalTableau[row, col];
                    newTableau[row, col] = Tableau[row, col];
                }
                else if (col == DecisionVars)
                {
                    newNonCanonicalTableau[row, col] = newColumnValues[row];
                    newTableau[row, col] = row == 0 ? -newColumnValues[row] : newColumnValues[row];
                }
                else if (col < newTotalVars)
                {
                    newNonCanonicalTableau[row, col] = NonCanonicalTableau[row, col - 1];
                    newTableau[row, col] = Tableau[row, col - 1];
                }
                else
                {
                    newNonCanonicalTableau[row, col] = NonCanonicalTableau[row, TotalVars];
                    newTableau[row, col] = Tableau[row, TotalVars];
                }
            }

        // As of now, assumming all values given have a sign restricition of +. May need to update
        string[] newSignRestrictions = new string[newDecisionVars];
        Array.Copy(SignRestrictions, 0, newSignRestrictions, 0, SignRestrictions.Length);
        newSignRestrictions[newDecisionVars - 1] = "+";

        // In order to use Math preliminary functions, a CanonicalTableau instance is required
        CanonicalTableau tempTableau = new(newDecisionVars, ExcessVars, SlackVars, IsMaximization, newSignRestrictions, newTableau, newNonCanonicalTableau);
        tempTableau.BasicVariableIndices = tempTableau.GetBasicVariableIndices();

        // Perform math preliminaries matrix creation and create a instance from the result
        MathPreliminariesResult result = tempTableau.ComputeMathPreliminaries(tempTableau.BasicVariableIndices);
        double[,] resultTable = tempTableau.ConstructMathPrelimOptimalTableau(result);
        CanonicalTableau resultInstance = new(newDecisionVars, ExcessVars, SlackVars, IsMaximization, newSignRestrictions, resultTable, newNonCanonicalTableau);

        // The result may no longer be optimal
        // TODO: Add check (negative values in Z - primal || negative values in RHS - dual)

        return resultInstance;
    }
    #endregion

    #region Utility Functions
    public bool IsSlackVariable(int columnIndex)
    {
        // Since slack is always added after excess. Check index between
        return columnIndex >= DecisionVars + ExcessVars && columnIndex < TotalVars;
    }

    public bool IsBasicVariable(int columnNo)
    {
        double[] column = GetColumn(columnNo);
        int oneCount = 0;
        int nonZeroCount = 0;
        foreach (double value in column)
        {
            if (value != 0)
            {
                nonZeroCount++;
            }
            if (value == 1)
            {
                oneCount++;
            }
        }
        return nonZeroCount == 1 && oneCount == 1;
    }

    public List<int> GetBasicVariableIndices()
    {
        List<int> xBVIndices = [];
        for (int row = 1; row < Rows; row++)
        {
            double[] rowValues = GetRow(row);
            for (int col = 0; col < TotalVars; col++)
                if (rowValues[col] == 1 && IsBasicVariable(col))
                {
                    xBVIndices.Add(col);
                    break;
                }
        }
        return xBVIndices;
    }

    public double[] GetRow(int rowNo, bool useNonCanonicalTableau = false)
    {
        double[,] usedTableau = useNonCanonicalTableau ? NonCanonicalTableau : Tableau;
        int rowCount = usedTableau.GetLength(0);
        int columnCount = usedTableau.GetLength(1);

        if (rowNo < 0 || rowNo >= rowCount)
            throw new ArgumentException($"Invalid row index provided: {rowNo}");

        double[] row = new double[columnCount];
        for (int col = 0; col < columnCount; col++)
        {
            row[col] = usedTableau[rowNo, col];
        }
        return row;
    }

    public double[] GetColumn(int columnNo, bool useNonCanonicalTableau = false)
    {
        double[,] usedTableau = useNonCanonicalTableau ? NonCanonicalTableau : Tableau;
        int columnCount = usedTableau.GetLength(1);
        int rowCount = usedTableau.GetLength(0);

        if (columnNo < 0 || columnNo >= columnCount)
            throw new ArgumentException($"Invalid column index provided: {columnNo}");

        double[] column = new double[Rows];
        for (int row = 0; row < rowCount; row++)
        {
            column[row] = Tableau[row, columnNo];
        }
        return column;
    }
    #endregion

    #region Math Preliminaries
    private double[,] GetB(List<int> xBVIndices)
    {
        double[,] b = new double[xBVIndices.Count, xBVIndices.Count];
        for (int row = 0; row < xBVIndices.Count; row++)
            for (int col = 0; col < xBVIndices.Count; col++)
                b[row, col] = NonCanonicalTableau[row + 1, xBVIndices[col]];
        return b;
    }

    private double[] GetCBV(List<int> xBVIndices)
    {
        double[] cBV = new double[xBVIndices.Count];
        for (int col = 0; col < xBVIndices.Count; col++)
            cBV[col] = NonCanonicalTableau[0, xBVIndices[col]];
        return cBV;
    }

    private static double[,] GetBInverse(double[,] b)
    {
        DenseMatrix bMatrix = DenseMatrix.OfArray(b);
        return bMatrix.Determinant() == 0
            ? throw new InvalidOperationException("B matrix is singular and cannot be inverted.")
            : bMatrix.Inverse().ToArray();
    }

    private static double[] GetCBV_BInverse(double[] cBV, double[,] bInverse)
    {
        DenseVector cBVVector = DenseVector.OfArray(cBV);
        return [.. cBVVector * DenseMatrix.OfArray(bInverse)];
    }

    private double[,] GetAjStarMatrix(List<int> xBVIndices, int totalVars)
    {
        DenseMatrix bMatrix = DenseMatrix.OfArray(GetB(xBVIndices));

        double[,] ajStarMatrix = new double[xBVIndices.Count, totalVars];
        for (int col = 0; col < totalVars; col++)
        {
            double[] aj = new double[xBVIndices.Count];
            for (int row = 0; row < xBVIndices.Count; row++)
                aj[row] = NonCanonicalTableau[row + 1, col];
            DenseVector ajVector = DenseVector.OfArray(aj);
            double[] ajStar = [.. bMatrix.Inverse().Multiply(ajVector)];
            for (int row = 0; row < xBVIndices.Count; row++)
                ajStarMatrix[row, col] = ajStar[row];
        }
        return ajStarMatrix;
    }

    private double[] GetCjStar(List<int> xBVIndices, double[] cBV_BInverse, int totalVars)
    {
        DenseVector cBV_BInvVector = DenseVector.OfArray(cBV_BInverse);

        double[] cjStar = new double[totalVars];
        for (int col = 0; col < totalVars; col++)
        {
            double cj = NonCanonicalTableau[0, col];
            double[] aj = new double[xBVIndices.Count];
            for (int row = 0; row < xBVIndices.Count; row++)
                aj[row] = NonCanonicalTableau[row + 1, col];
            DenseVector ajVector = DenseVector.OfArray(aj);
            cjStar[col] = cBV_BInvVector.DotProduct(ajVector) - cj;
        }
        return cjStar;
    }

    private double[] GetBStar(List<int> xBVIndices)
    {
        DenseMatrix bMatrix = DenseMatrix.OfArray(GetB(xBVIndices));

        double[] bVector = new double[xBVIndices.Count];
        for (int row = 0; row < xBVIndices.Count; row++)
            bVector[row] = NonCanonicalTableau[row + 1, TotalVars];
        DenseVector bColumnVector = DenseVector.OfArray(bVector);
        return [.. bMatrix.Inverse().Multiply(bColumnVector)];
    }

    private static double GetZStar(double[] cBV, double[] bStar)
    {
        return DenseVector.OfArray(cBV).DotProduct(DenseVector.OfArray(bStar));
    }

    private double[,] ConstructMathPrelimOptimalTableau(MathPreliminariesResult calc, double[]? newColumnValues = null)
    {
        int totalVars = newColumnValues != null ? TotalVars + 1 : TotalVars;
        double[,] optimalTableau = new double[Rows, totalVars + 1];
        for (int col = 0; col < TotalVars; col++)
            optimalTableau[0, col] = calc.CjStar[col];
        if (newColumnValues != null)
            optimalTableau[0, TotalVars] = calc.CjStar[TotalVars];
        optimalTableau[0, totalVars] = calc.ZStar;
        for (int row = 0; row < calc.BasicVariableIndices.Count; row++)
            for (int col = 0; col < totalVars; col++)
                optimalTableau[row + 1, col] = calc.AjStar[row, col];
        for (int row = 0; row < calc.BasicVariableIndices.Count; row++)
            optimalTableau[row + 1, totalVars] = calc.BStar[row];
        return optimalTableau;
    }

    private MathPreliminariesResult ComputeMathPreliminaries(List<int> xBVIndices)
    {
        if (xBVIndices.Count == 0)
            throw new InvalidOperationException("No basic variables found in table");

        double[,] b = GetB(xBVIndices);
        double[] cBV = GetCBV(xBVIndices);
        double[,] bInverse = GetBInverse(b);
        double[] cBV_BInverse = GetCBV_BInverse(cBV, bInverse);
        double[,] ajStar = GetAjStarMatrix(xBVIndices, TotalVars);
        double[] cjStar = GetCjStar(xBVIndices, cBV_BInverse, TotalVars);
        double[] bStar = GetBStar(xBVIndices);
        double zStar = GetZStar(cBV, bStar);

        return new MathPreliminariesResult
        {
            BasicVariableIndices = xBVIndices,
            B = b,
            CBV = cBV,
            BInverse = bInverse,
            CBV_BInverse = cBV_BInverse,
            AjStar = ajStar,
            CjStar = cjStar,
            BStar = bStar,
            ZStar = zStar
        };
    }
    #endregion
}
