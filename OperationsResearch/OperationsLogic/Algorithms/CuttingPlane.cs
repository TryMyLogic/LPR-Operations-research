using System.Text;

using OperationsLogic.Misc;
using OperationsLogic.Model;

namespace OperationsLogic.Algorithms;

public class CuttingPlane : ISolver
{
    public void Solve(LinearModel model, out string output)
    {
        CanonicalTableau tableau = ConvertToCanonicalTableau(model);
        output = Solve(tableau);
    }

    private static CanonicalTableau ConvertToCanonicalTableau(LinearModel model)
    {
        // Essentially performing the same setup as in the constructor of CanonicalTableau
        ArgumentNullException.ThrowIfNull(model);

        if (model.ObjectiveCoefficients == null || model.Constraints == null || model.SignRestrictions == null)
            throw new ArgumentException("Model components cannot be null.");
        if (model.ObjectiveCoefficients.Count == 0)
            throw new ArgumentException("Objective function must have at least one coefficient.");
        if (model.Constraints.Count == 0)
            throw new ArgumentException("Model must have at least one constraint.");
        if (model.SignRestrictions.Length != model.ObjectiveCoefficients.Count)
            throw new ArgumentException("Sign restrictions must match the number of decision variables.");

        int decisionVars = model.ObjectiveCoefficients.Count;
        int numConstraints = model.Constraints.Count;
        int slackVars = model.Constraints.Count(c => c.Relation == "<=") + model.Constraints.Count(c => c.Relation == "=");
        int excessVars = model.Constraints.Count(c => c.Relation == ">=") + model.Constraints.Count(c => c.Relation == "=");
        int totalVars = decisionVars + excessVars + slackVars;
        int rows = numConstraints + 1;

        double[,] tableau = new double[rows, totalVars + 1];
        double[,] nonCanonicalTableau = new double[rows, totalVars + 1];

        bool isMaximization = model.Type.Equals("max", StringComparison.CurrentCultureIgnoreCase);
        for (int col = 0; col < decisionVars; col++)
        {
            tableau[0, col] = isMaximization ? -model.ObjectiveCoefficients[col] : model.ObjectiveCoefficients[col];
            nonCanonicalTableau[0, col] = model.ObjectiveCoefficients[col];
        }
        tableau[0, totalVars] = 0;
        nonCanonicalTableau[0, totalVars] = 0;

        int slackIndex = 0;
        int excessIndex = 0;
        for (int row = 0; row < numConstraints; row++)
        {
            Constraint constraint = model.Constraints[row];
            if (constraint.Coefficients == null)
                throw new ArgumentException($"Constraint {row + 1} has null coefficients.");
            if (constraint.Coefficients.Count > decisionVars)
                throw new ArgumentException($"Constraint {row + 1} has too many coefficients.");
            if (!new[] { "<=", ">=", "=" }.Contains(constraint.Relation))
                throw new ArgumentException($"Invalid relation in constraint {row + 1}: {constraint.Relation}");

            for (int col = 0; col < decisionVars; col++)
            {
                double coeff = col < constraint.Coefficients.Count ? constraint.Coefficients[col] : 0;
                tableau[row + 1, col] = constraint.Relation == ">=" ? -coeff : coeff;
                nonCanonicalTableau[row + 1, col] = coeff;
            }

            tableau[row + 1, totalVars] = constraint.Relation == ">=" ? -constraint.RHS : constraint.RHS;
            nonCanonicalTableau[row + 1, totalVars] = constraint.RHS;

            if (constraint.Relation == "<=")
            {
                tableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                slackIndex++;
            }
            else if (constraint.Relation == ">=")
            {
                tableau[row + 1, decisionVars + excessIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessIndex] = -1;
                excessIndex++;
            }
            else if (constraint.Relation == "=")
            {
                tableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessVars + slackIndex] = 1;
                tableau[row + 1, decisionVars + excessIndex] = 1;
                nonCanonicalTableau[row + 1, decisionVars + excessIndex] = -1;
                slackIndex++;
                excessIndex++;
            }
        }

        string[] validOperators = { "+", "-", "urs", "int", "bin" };
        foreach (string restriction in model.SignRestrictions)
        {
            if (!validOperators.Contains(restriction))
                throw new ArgumentException($"Invalid sign restriction: {restriction}");
        }

        CanonicalTableau resultTableau = new(
            decisionVars: decisionVars,
            excessVars: excessVars,
            slackVars: slackVars,
            isMaximization: isMaximization,
            signRestrictions: model.SignRestrictions,
            tableau: tableau,
            noncanonicaltableau: nonCanonicalTableau
        );

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < totalVars + 1; col++)
            {
                if (double.IsNaN(tableau[row, col]) || double.IsInfinity(tableau[row, col]))
                    throw new ArgumentException($"Invalid value in Tableau at [{row},{col}]: NaN or Infinity.");
                if (double.IsNaN(nonCanonicalTableau[row, col]) || double.IsInfinity(nonCanonicalTableau[row, col]))
                    throw new ArgumentException($"Invalid value in NonCanonicalTableau at [{row},{col}]: NaN or Infinity.");
            }
        }

        _ = resultTableau.GetBasicVariableIndices();

        return resultTableau;
    }

    private static LinearModel ConvertToLinearModel(CanonicalTableau tableau)
    {
        List<double> objectiveCoeffs = [];
        for (int i = 0; i < tableau.DecisionVars; i++)
        {
            objectiveCoeffs.Add(tableau.NonCanonicalTableau[0, i]);
        }

        List<Constraint> constraints = [];
        for (int row = 1; row < tableau.Rows; row++)
        {
            List<double> coeffs = [];

            for (int col = 0; col < tableau.DecisionVars; col++)
            {
                coeffs.Add(tableau.NonCanonicalTableau[row, col]);
            }

            double rhs = tableau.NonCanonicalTableau[row, tableau.TotalVars];

            string relation;
            if (tableau.IsSlackVariable(tableau.DecisionVars + tableau.ExcessVars + row - 1))
                relation = "<=";
            else if (tableau.IsSlackVariable(tableau.DecisionVars + row - 1))
                relation = "=";
            else
                relation = ">=";

            constraints.Add(new Constraint(coeffs, relation, rhs));
        }

        return new LinearModel(
            type: tableau.IsMaximization ? "max" : "min",
            objectiveCoefficients: objectiveCoeffs,
            constraints: constraints,
            signRestrictions: tableau.SignRestrictions
        );
    }

    public static string Solve(CanonicalTableau tableau)
    {
        StringBuilder sb = new();
        int iteration = 0;

        while (true)
        {
            var (isOptimal, mode) = CanonicalTableau.IsOptimal(tableau);

            if (!isOptimal)
            {
                sb.AppendLine("The table is not optimal. Optimizing");
                iteration++;

                CanonicalTableau? iterTableau;

                if (mode == "Primal Simplex")
                {
                    LinearModel linearModel = ConvertToLinearModel(tableau);
                    SimplexSolver primalSolver = new();
                    primalSolver.Solve(linearModel, out string primalOutput);
                    double[,] updatedTableau = primalSolver.FinalTableau;

                    int rows = updatedTableau.GetLength(0);
                    int cols = updatedTableau.GetLength(1);
                    double[,] displayTableau = (double[,])updatedTableau.Clone();

                    // Swap Z row (last) to top since Josh has a preference for putting Z in the last row
                    for (int col = 0; col < cols; col++)
                    {
                        (displayTableau[rows - 1, col], displayTableau[0, col]) = (displayTableau[0, col], displayTableau[rows - 1, col]);
                    }

                    iterTableau = new CanonicalTableau(
                    decisionVars: tableau.DecisionVars,
                    excessVars: tableau.ExcessVars,
                    slackVars: tableau.SlackVars,
                    isMaximization: tableau.IsMaximization,
                    signRestrictions: tableau.SignRestrictions,
                    tableau: displayTableau,
                     noncanonicaltableau: tableau.NonCanonicalTableau
                    );

                    sb.AppendLine(primalOutput);
                }
                else
                {
                    (_, iterTableau) = DualSimplex.Solve(tableau, iteration);
                }

                tableau = iterTableau ?? throw new InvalidOperationException("Return table should not be null!");

                sb.AppendLine(tableau.DisplayTableau());
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            int cutRowIndex = DetermineCuttingRowIndex(tableau);

            if (iteration == 0)
            {
               sb.AppendLine("Initial Cutting Plane Tableau:");
            }

            if (cutRowIndex == -1)
            {
               sb.AppendLine($"Iteration {iteration}");
                sb.AppendLine("No further cuts needed or no valid cutting row found.");
                break;
            }

            string msg = $"Selected cutting row: {cutRowIndex}, RHS: {tableau.Tableau[cutRowIndex, tableau.TotalVars]}, Fractional part: {Math.Abs(tableau.Tableau[cutRowIndex, tableau.TotalVars] - Math.Floor(tableau.Tableau[cutRowIndex, tableau.TotalVars]))}";
            sb.AppendLine(msg);
            _ = sb.AppendLine();

            sb.AppendLine($"Iteration {iteration}");
            double[] newConstraint = DetermineCuttingConstraint(tableau, cutRowIndex);
            tableau = AddCutToTableau(tableau, newConstraint);
            sb.AppendLine(tableau.DisplayTableau());

            iteration++; // Count Dual Simplex as separate Iteration
            (string output, CanonicalTableau? iter) = DualSimplex.Solve(tableau, iteration);
            tableau = iter ?? throw new InvalidOperationException("Return table should not be null!");

            sb.AppendLine(output);
            iteration++;
        }
        return sb.ToString();
    }

    public static int DetermineCuttingRowIndex(CanonicalTableau tableau)
    {
        double target = 0.5;
        int cutRowIndex = -1;
        double minDistanceToTarget = double.MaxValue;

        for (int col = 0; col < tableau.DecisionVars; col++)
        {
            // You only cut non-integers
            if (tableau.SignRestrictions[col] != "int")
            {
                continue;
            }

            if (tableau.IsBasicVariable(col))
            {
                // Determine row where basic variable has 1
                for (int row = 1; row < tableau.Rows; row++)
                {
                    if (tableau.Tableau[row, col] == 1)
                    {
                        double rhs = tableau.Tableau[row, tableau.TotalVars];

                        double fractionalPart = Math.Abs(rhs - Math.Floor(rhs));

                        // Only consider decimal part
                        if (fractionalPart > 0)
                        {
                            double distanceToTarget = Math.Abs(fractionalPart - target);
                            if (distanceToTarget < minDistanceToTarget)
                            {
                                minDistanceToTarget = distanceToTarget;
                                cutRowIndex = row;
                            }
                        }
                        break;
                    }
                }
            }
        }

        return cutRowIndex;
    }

    public static double[] DetermineCuttingConstraint(CanonicalTableau tableau, int cutRowIndex)
    {
        // New double is not zero indexed hence - +1
        double[] newConstraint = new double[tableau.TotalVars + 2]; // +1 for new slack/excess column.

        double rhs = tableau.Tableau[cutRowIndex, tableau.TotalVars];
        double fractionalRhs = Math.Abs(rhs - Math.Floor(rhs));

        double totalFractional = fractionalRhs;
        for (int col = 0; col < tableau.TotalVars; col++)
        {
            double coeff = tableau.Tableau[cutRowIndex, col];
            double fractionalPart = coeff - Math.Floor(coeff);

            if (fractionalPart < 0)
            {
                fractionalPart = 1 + fractionalPart; // Handle negative coefficients
            }

            newConstraint[col] = fractionalPart == 0 ? 0 : -fractionalPart;
            totalFractional += fractionalPart;
        }

        newConstraint[tableau.TotalVars] = 1;
        newConstraint[tableau.TotalVars + 1] = -fractionalRhs;

        return newConstraint;
    }

    public static CanonicalTableau AddCutToTableau(CanonicalTableau tableau, double[] newConstraint)
    {
        double[,] newTableau = new double[tableau.Rows + 1, tableau.TotalVars + 2];

        for (int row = 0; row < tableau.Rows; row++)
        {
            for (int col = 0; col < tableau.TotalVars + 1; col++)
            {
                // If its the RHS col, shift it to the right by 1 else copy
                if (col == tableau.TotalVars)
                    newTableau[row, col + 1] = tableau.Tableau[row, col];
                else
                    newTableau[row, col] = tableau.Tableau[row, col];
            }
        }

        for (int col = 0; col < newConstraint.Length; col++)
        {
            newTableau[tableau.Rows, col] = newConstraint[col];
        }

        string[] newSignRestrictions = new string[tableau.DecisionVars + 1];
        Array.Copy(tableau.SignRestrictions, newSignRestrictions, tableau.DecisionVars);
        newSignRestrictions[tableau.DecisionVars] = "+"; // New slack variable sign

        return new CanonicalTableau(
            decisionVars: tableau.DecisionVars,
            excessVars: tableau.ExcessVars,
            slackVars: tableau.SlackVars + 1,
            isMaximization: tableau.IsMaximization,
            signRestrictions: newSignRestrictions,
            noncanonicaltableau: tableau.NonCanonicalTableau,
            tableau: newTableau
        );
    }
}


