using System.Text;

using OperationsLogic.Misc;
using OperationsLogic.Model;

namespace OperationsLogic.Algorithms;

public class CuttingPlane : ISolver
{
    public void Solve(LinearModel model, out string output)
    {
        CanonicalTableau tableau = ModelConverter.ConvertToCanonicalTableau(model);
        output = Solve(tableau);
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
                    LinearModel linearModel = ModelConverter.ConvertToLinearModel(tableau);
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


