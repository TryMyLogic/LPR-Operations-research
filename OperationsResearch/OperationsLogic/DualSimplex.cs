using System.Data;

namespace OperationsLogic;

public class DualSimplex
{
    public static void Solve(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {
        bool solutionReached = false;

        int iteration = 0;
        do
        {
            int pivotRowIndex = SimplexUtils.FindMostNegativeRHSIndex(rhsValues);

            if (pivotRowIndex != -1)
            {
                Console.WriteLine($"\nIteration {iteration}");
                iteration++;
                (xValues, rhsValues, sValues, eValues) = PivotOnTable(xValues, rhsValues, sValues, eValues);
                SimplexUtils.DisplayTable(xValues, rhsValues, sValues, eValues);
            }
            else
            {
                solutionReached = true;
            }
        }
        while (!solutionReached);
    }
    public static (double[,] xValues, double[] rhsValues, double[,]? sValues, double[,]? eValues) PivotOnTable(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {
        int xColumnCount = xValues.GetLength(1);

        int sColumnCount = 0;
        if (sValues != null)
        {
            sColumnCount = sValues.GetLength(1);
        }

        int eColumnCount = 0;
        if (eValues != null)
        {
            eColumnCount = eValues.GetLength(1);
        }

        int pivotRowIndex = SimplexUtils.FindMostNegativeRHSIndex(rhsValues);

        // If an index is found, begin pivoting
        if (pivotRowIndex != -1)
        {
            int totalColumnCount = xColumnCount + sColumnCount + eColumnCount;
            double[] thetaValues = new double[totalColumnCount];
            int thetaIndex = 0;
            double thetaValue;
            for (int col = 0; col < xColumnCount; col++)
            {
                if (xValues[pivotRowIndex, col] < 0)
                {
                    double zValue = xValues[0, col];
                    thetaValue = Math.Abs(zValue / xValues[pivotRowIndex, col]);
                    thetaValues[thetaIndex] = thetaValue;
                }
                thetaIndex++;
            }

            for (int col = 0; col < sColumnCount; col++)
            {
                if (sValues != null && sValues[pivotRowIndex, col] < 0)
                {
                    thetaValue = Math.Abs(sValues[0, col] / sValues[pivotRowIndex, col]);
                    thetaValues[thetaIndex] = thetaValue;
                    thetaIndex++;
                }
            }

            for (int col = 0; col < eColumnCount; col++)
            {
                if (eValues != null && eValues[pivotRowIndex, col] < 0)
                {
                    thetaValue = Math.Abs(eValues[0, col] / eValues[pivotRowIndex, col]);
                    thetaValues[thetaIndex] = thetaValue;
                    thetaIndex++;
                }
            }

            for (int i = 0; i < thetaValues.Length; i++)
            {
                Console.WriteLine($"thetaValues[{i}] = {thetaValues[i]}");
            }

            // It finds the correct column but since we have x, s and e values split, may give index out of bounds if not an xValue. 
            int pivotColumnIndex = SimplexUtils.FindLeastPositiveThetaIndex(thetaValues);
            if (pivotColumnIndex != -1)
            {
                (double[,] updatedX, double[] updatedRhs, double[,]? updatedS, double[,]? updatedE) = SimplexUtils.PerformPivot(pivotRowIndex, pivotColumnIndex, xValues, rhsValues, sValues, eValues);
                return (updatedX, updatedRhs, updatedS, updatedE);
            }
            else
            {
                // Likely Infeasible
                return (xValues, rhsValues, sValues, eValues);
            }
        }
        else
        {
            // Determine if table is optimal
            return (xValues, rhsValues, sValues, eValues);
        }
    }
}
