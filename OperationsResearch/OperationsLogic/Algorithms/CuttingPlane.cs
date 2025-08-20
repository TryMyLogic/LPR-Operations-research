using System.Text;

using OperationsLogic.Model;

namespace OperationsLogic.Algorithms;

public class CuttingPlane
{
    public static string Solve(CanonicalTableau tableau)
    {
        StringBuilder sb = new();

        int iteration = 0;
        while (true)
        {
            int cutRowIndex = DetermineCuttingRowIndex(tableau);

            if (iteration == 0)
            {
                _ = sb.AppendLine("Initial Cutting Plane Tableau:");
            }

            if (cutRowIndex == -1)
            {
                _ = sb.AppendLine($"Iteration {iteration}");
                _ = sb.AppendLine("No further cuts needed or no valid cutting row found.");
                break;
            }

            string msg = $"Selected cutting row: {cutRowIndex}, RHS: {tableau.Tableau[cutRowIndex, tableau.TotalVars]}, Fractional part: {Math.Abs(tableau.Tableau[cutRowIndex, tableau.TotalVars] - Math.Floor(tableau.Tableau[cutRowIndex, tableau.TotalVars]))}";
            sb.AppendLine(msg);
            sb.AppendLine();

            _ = sb.AppendLine($"Iteration {iteration}");
            double[] newConstraint = DetermineCuttingConstraint(tableau, cutRowIndex, 0);
            tableau = AddCutToTableau(tableau, newConstraint);
            _ = sb.AppendLine(tableau.DisplayTableau());

            iteration++; // Count Dual Simplex as seperate Iteration
            (string output, CanonicalTableau? iter) = DualSimplex.Solve(tableau, iteration);
            tableau = iter ?? throw new InvalidOperationException("Return table should not be null!");

            _ = sb.AppendLine(output);
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

            double[] column = GetColumn(tableau.Tableau, col, tableau.Rows);
            if (IsBasicVariable(column))
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

    public static double[] DetermineCuttingConstraint(CanonicalTableau tableau, int cutRowIndex, int basicVarIndex)
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
            tableau: newTableau
        );
    }

    private static double[] GetColumn(double[,] tableau, int columnNo, int rowCount)
    {
        double[] column = new double[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            column[i] = tableau[i, columnNo];
        }
        return column;
    }

    private static bool IsBasicVariable(double[] column)
    {
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

}


