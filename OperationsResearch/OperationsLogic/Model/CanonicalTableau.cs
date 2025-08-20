using System.Text;

namespace OperationsLogic.Model;
public class CanonicalTableau
{
    public int Rows { get; }
    public int DecisionVars { get; }
    public int ExcessVars { get; }
    public int SlackVars { get; }
    public int TotalVars => DecisionVars + ExcessVars + SlackVars; // Column count
    public double[,] Tableau { get; private set; } // Matrix style table
    public bool IsMaximization { get; }
    public string[] SignRestrictions { get; }  // specify +, -, urs, int or bin for each decision var in inputLines

    private static readonly string[] ValidOperatorsAndTypes = ["+", "-", "urs", "int", "bin"];
    private static readonly string[] ValidConstraintOperators = ["<=", ">=", "="];

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
        //Console.WriteLine(DecisionVars);
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

        /* Enter values */
        for (int decisionVar = 0; decisionVar < DecisionVars; decisionVar++)
            Tableau[0, decisionVar] = IsMaximization ? -objCoefficients[decisionVar] : objCoefficients[decisionVar];
        Tableau[0, TotalVars] = 0; // RHS is always 0 for Obj Func

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
            for (int j = 0; j < DecisionVars; j++)
            {
                if (j < coeffCount)
                {
                    string term = parts[j];  // e.g., +2
                    if (string.IsNullOrEmpty(term) || (term[0] != '+' && term[0] != '-'))
                        throw new ArgumentException($"Invalid operator in constraint {row + 1}: {term}");
                    if (!double.TryParse(term[1..], out double coeff))
                        throw new ArgumentException($"Invalid coefficient in constraint {row + 1}: {term}");
                    Tableau[row + 1, j] = parts[^2] == ">=" ? -(term[0] == '+' ? coeff : -coeff) : term[0] == '+' ? coeff : -coeff;
                }
                else
                    Tableau[row + 1, j] = 0;  // Implied 0 for missing coefficients
            }

            // RHS
            if (!double.TryParse(parts[^1], out double rhs)) // ^1 means count from last decending
                throw new ArgumentException($"Invalid RHS in constraint {row + 1}: {parts[^1]}");
            Tableau[row + 1, TotalVars] = parts[^2] == ">=" ? -rhs : rhs;

            // Slack/excess variables
            string relation = parts[^2];
            if (relation == "<=")
            {
                Tableau[row + 1, DecisionVars + ExcessVars + slackIndex] = 1;  // Slack variable
                slackIndex++;
            }
            else if (relation == ">=")
            {
                Tableau[row + 1, DecisionVars + excessIndex] = 1;  // Excess variable
                excessIndex++;
            }
            else if (relation == "=")
            {
                // Unsure if i did URS correctly. Need to test
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
            for (int col = 0; col < TotalVars + 1; col++)
                if (double.IsNaN(Tableau[row, col]) || double.IsInfinity(Tableau[row, col]))
                    throw new ArgumentException($"Invalid value at [{row},{col}]: NaN or Infinity.");
    }

    public string DisplayTableau(double[]? thetas = null, bool printToConsole = false)
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

        for (int row = 0; row < Rows; row++)
        {
            _ = sb.Append(row == 0 ? "Z\t" : $"{row}\t");
            for (int col = 0; col < TotalVars + 1; col++)
                _ = sb.Append($"{Math.Round(Tableau[row, col], 3)}\t");
            _ = sb.Append('\n');
        }

        if (thetas != null)
        {
            _ = sb.Append("Theta\t");
            for (int col = 0; col < TotalVars; col++)
                _ = sb.Append($"{Math.Round(thetas[col], 3)}\t");
            _ = sb.AppendLine();
        }

        if (printToConsole)
            Console.Write(sb.ToString());

        return sb.ToString();
    }


    // the FindPivotRow needs to be made by as seperate model, e.g DualSimplex which uses CanonicalTablue as a param.
    // How you pivot on dual simplex is not the same as primal simplex
    //public int FindPivotRow()
    //{

    //}

    // Return new Tableau so we can call Display on each iteration.
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

        return new CanonicalTableau(DecisionVars, ExcessVars, SlackVars, IsMaximization, SignRestrictions, newTableau);
    }

    private CanonicalTableau(int decisionVars, int excessVars, int slackVars, bool isMaximization, string[] signRestrictions, double[,] tableau)
    {
        DecisionVars = decisionVars;
        ExcessVars = excessVars;
        SlackVars = slackVars;
        Rows = tableau.GetLength(0);
        Tableau = tableau;
        IsMaximization = isMaximization;
        SignRestrictions = signRestrictions;
    }
}
