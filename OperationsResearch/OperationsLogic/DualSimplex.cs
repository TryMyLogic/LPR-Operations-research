namespace OperationsLogic;

public class DualSimplex
{
    public static void Solve(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {
        bool solutionReached = false;

        int iteration = 1;
        do
        {
            Console.WriteLine($"\nIteration {iteration}");

            int pivotRowIndex = SimplexUtils.FindMostNegativeRHSIndex(rhsValues);

            if (pivotRowIndex != -1)
            {
                (xValues, rhsValues, sValues, eValues, _) = PivotOnTable(xValues, rhsValues, sValues, eValues);
                SimplexUtils.DisplayTable(xValues, rhsValues, sValues, eValues);
                iteration++;
            }
            else
            {
                Console.WriteLine("Optimal: No negative RHS values");
                SimplexUtils.DisplayTable(xValues, rhsValues, sValues, eValues, null);
                solutionReached = true;
            }
        }
        while (!solutionReached);
    }
    public static (double[,] xValues, double[] rhsValues, double[,]? sValues, double[,]? eValues, double[,]? thetaValues) PivotOnTable(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {
        int xColumnCount = xValues.GetLength(1);
        int sColumnCount = sValues != null ? sValues.GetLength(1) : 0;
        int eColumnCount = eValues != null ? eValues.GetLength(1) : 0;
        int maxColumnCount = Math.Max(Math.Max(xColumnCount, sColumnCount), eColumnCount);

        int pivotRowIndex = SimplexUtils.FindMostNegativeRHSIndex(rhsValues);

        // If an index is found, begin pivoting
        if (pivotRowIndex != -1)
        {
            double[,] thetaValues = new double[3, maxColumnCount]; // Row 0: X, Row 1: S, Row 2: E
            double thetaValue;
            for (int col = 0; col < xColumnCount; col++)
            {
                if (xValues[pivotRowIndex, col] is < 0 and not 0)
                {
                    double zValue = xValues[0, col];
                    thetaValue = Math.Abs(zValue / xValues[pivotRowIndex, col]);

                    // Ensures any NAN values and infinite values do not slip through by setting them to 0
                    thetaValues[0, col] = double.IsNaN(thetaValue) || double.IsInfinity(thetaValue) ? 0 : thetaValue;
                }
            }

            for (int col = 0; col < sColumnCount; col++)
            {
                if (sValues != null && sValues[pivotRowIndex, col] < 0 && sValues[pivotRowIndex, col] != 0)
                {
                    thetaValue = Math.Abs(sValues[0, col] / sValues[pivotRowIndex, col]);
                    thetaValues[1, col] = double.IsNaN(thetaValue) || double.IsInfinity(thetaValue) ? 0 : thetaValue;
                }
            }

            for (int col = 0; col < eColumnCount; col++)
            {
                if (eValues != null && eValues[pivotRowIndex, col] < 0 && eValues[pivotRowIndex, col] != 0)
                {
                    thetaValue = Math.Abs(eValues[0, col] / eValues[pivotRowIndex, col]);
                    thetaValues[2, col] = double.IsNaN(thetaValue) || double.IsInfinity(thetaValue) ? 0 : thetaValue;
                }
            }

            (int pivotColumnIndex, SimplexUtils.ColumnGroup columnGroup) = SimplexUtils.FindLeastPositiveThetaIndex(thetaValues, xColumnCount, sColumnCount, eColumnCount);
            if (pivotColumnIndex != -1)
            {
                (double[,] updatedX, double[] updatedRhs, double[,]? updatedS, double[,]? updatedE) = SimplexUtils.PerformPivot(pivotRowIndex, pivotColumnIndex, xValues, rhsValues, sValues, eValues, columnGroup);
                return (updatedX, updatedRhs, updatedS, updatedE, thetaValues);
            }
            else
            {
                Console.WriteLine("Infeasible: No valid pivot column found");
                return (xValues, rhsValues, sValues, eValues, thetaValues);
            }
        }
        else
        {
            Console.WriteLine("Optimal: No negative RHS values");
            return (xValues, rhsValues, sValues, eValues, null);
        }
    }
}
