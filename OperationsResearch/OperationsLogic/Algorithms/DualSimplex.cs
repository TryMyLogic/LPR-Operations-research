using System.Text;

using OperationsLogic.Model;

namespace OperationsLogic.Algorithms;

public class DualSimplex
{
    public static (string, CanonicalTableau?) Solve(CanonicalTableau tableau, int iteration = 0)
    {
        StringBuilder sb = new();

        int initialIteration = iteration;
        while (true)
        {
            int pivotRowIndex = FindMostNegativeRHSIndex(tableau);

            if (pivotRowIndex == -1)
            {
                _ = sb.AppendLine($"Iteration {iteration}");
                _ = sb.AppendLine("Optimal: No negative RHS values");
                _ = sb.AppendLine(tableau.DisplayTableau());
                break;
            }

            (int pivotColumn, double[] thetas) = DeterminePivotColumnAndThetas(tableau, pivotRowIndex);

            if (iteration == initialIteration)
            {
                _ = sb.AppendLine("Initial Tableau:");
                _ = sb.AppendLine(tableau.DisplayTableau(thetas));
                _ = sb.AppendLine("Initial Dual Simplex Tableau:");
                _ = sb.AppendLine($"Iteration {iteration}");
                _ = sb.AppendLine(tableau.DisplayTableau(thetas));
            }
            else
            {
                _ = sb.AppendLine($"Iteration {iteration}");
                _ = sb.AppendLine(tableau.DisplayTableau(thetas));
            }

            if (pivotColumn == -1)
            {
                _ = sb.AppendLine("Infeasible: No valid pivot column found");
                break;
            }

            tableau = tableau.PerformPivot(pivotRowIndex, pivotColumn);
            iteration++;
        }

        return (sb.ToString(), tableau);
    }

    public static int FindMostNegativeRHSIndex(CanonicalTableau tableau)
    {
        double mostNegative = 0;
        int pivotRow = -1; // Previously called negativeIndex
        for (int row = 1; row < tableau.Rows; row++) // Starts at 1 since you dont check Z row RHS (always 0)
        {
            double rhsValue = tableau.Tableau[row, tableau.TotalVars];
            if (rhsValue < mostNegative)
            {
                mostNegative = rhsValue;
                pivotRow = row;
            }
        }
        return pivotRow;
    }

    // Function previously known as FindLeastPositiveThetaIndex
    // Requires FindMostNegativeRHSIndex to be run first
    public static (int pivotColumn, double[] thetas) DeterminePivotColumnAndThetas(CanonicalTableau tableau, int pivotRow)
    {
        double[] thetas = new double[tableau.TotalVars];
        int pivotColumnIndex = -1;
        double leastPositive = double.PositiveInfinity;

        for (int column = 0; column < tableau.TotalVars; column++)
        {
            // Ensure the row value is negative. Only negative rowValues may have theta
            if (tableau.Tableau[pivotRow, column] < 0)
            {
                double theta = Math.Abs(tableau.Tableau[0, column] / tableau.Tableau[pivotRow, column]);
                thetas[column] = double.IsNaN(theta) || double.IsInfinity(theta) ? 0 : theta;
                if (thetas[column] > 0 && thetas[column] < leastPositive)
                {
                    leastPositive = thetas[column];
                    pivotColumnIndex = column;
                }
            }
        }

        return (pivotColumnIndex, thetas);
    }
}
