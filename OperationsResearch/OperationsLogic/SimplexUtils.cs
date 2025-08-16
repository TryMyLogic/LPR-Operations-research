using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic;
public class SimplexUtils
{
    public static void DisplayTable(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null, double[]? thetaValues = null)
    {
        int xColumnCount = xValues.GetLength(1);
        int sColumnCount = sValues?.GetLength(1) ?? 0;
        int eColumnCount = eValues?.GetLength(1) ?? 0;

        // Labels
        Console.Write("T\t");
        for (int i = 0; i < xColumnCount; i++)
        {
            Console.Write($"x{i + 1}\t");
        }
        for (int i = 0; i < eColumnCount; i++)
        {
            Console.Write($"E{i + 1}\t");
        }
        for (int i = 0; i < sColumnCount; i++)
        {
            Console.Write($"S{i + 1}\t");
        }
        Console.WriteLine("RHS");

        // Objective Function Values (First Row)
        Console.Write("Z\t");
        for (int i = 0; i < xColumnCount; i++)
        {
            Console.Write($"{xValues[0, i]}\t");
        }
        for (int i = 0; i < eColumnCount; i++)
        {
            Console.Write($"{eValues?[0, i] ?? 0}\t");
        }
        for (int i = 0; i < sColumnCount; i++)
        {
            Console.Write($"{sValues?[0, i] ?? 0}\t");
        }
        Console.WriteLine($"{rhsValues[0]}");

        // Display Remaining Rows
        for (int row = 1; row < xValues.GetLength(0); row++)
        {
            Console.Write($"{row}\t");
            for (int col = 0; col < xColumnCount; col++)
            {
                Console.Write($"{xValues[row, col]}\t");
            }
            for (int col = 0; col < eColumnCount; col++)
            {
                Console.Write($"{eValues?[row, col] ?? 0}\t");
            }
            for (int col = 0; col < sColumnCount; col++)
            {
                Console.Write($"{sValues?[row, col] ?? 0}\t");
            }
            Console.WriteLine($"{rhsValues[row]}");
        }

        // Display Theta Values (If Any)
        // TODO
    }

    public static (double[,] xValues, double[] rhsValues, double[,]? sValues, double[,]? eValues) PerformPivot(int pivotRowIndex, int pivotColumnIndex, double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {
        double[,] updatedXValues = new double[xValues.GetLength(0), xValues.GetLength(1)];
        double[] updatedRhsValues = new double[rhsValues.Length];
        double[,]? updatedSValues = sValues != null ? new double[sValues.GetLength(0), sValues.GetLength(1)] : null;
        double[,]? updatedEValues = eValues != null ? new double[eValues.GetLength(0), eValues.GetLength(1)] : null;
        
        double pivotIntersectionValue = xValues[pivotRowIndex, pivotColumnIndex];

        for (int row = 0; row < xValues.GetLength(0); row++)
        {
            for (int col = 0; col < xValues.GetLength(1); col++)
            {
                double newValue;
                if (row == pivotRowIndex)
                {
                    newValue = xValues[row, col] / pivotIntersectionValue;
                }
                else
                {
                    // (xValues[pivotRowIndex, col] / pivotIntersectionValue) is the same as if row == pivotRowIndex newValue and done this way to avoid circuler reference
                    newValue = xValues[row, col] - (xValues[row, pivotColumnIndex] * (xValues[pivotRowIndex, col] / pivotIntersectionValue));
                }        

                // The additional check fixes issues where negative zero and positive zero appear and make them zero
                updatedXValues[row, col] = newValue == 0 ? 0 : newValue;
            }

            if (sValues != null && updatedSValues != null)
            {
                for (int col = 0; col < sValues.GetLength(1); col++)
                {
                    // Every newValue from here onwards uses ternary for conciseness. Above is left as is as its easier to read and understand for some. 
                    double newValue = row == pivotRowIndex ? sValues[row, col] / pivotIntersectionValue : sValues[row, col] - (xValues[row, pivotColumnIndex] * (sValues[pivotRowIndex, col] / pivotIntersectionValue));
                    updatedSValues[row, col] = newValue == 0 ? 0 : newValue;
                }
            }

            if (eValues != null && updatedEValues != null)
            {
                for (int col = 0; col < eValues.GetLength(1); col++)
                {
                    double newValue = row == pivotRowIndex ? eValues[row, col] / pivotIntersectionValue : eValues[row, col] - (xValues[row, pivotColumnIndex] * (eValues[pivotRowIndex, col] / pivotIntersectionValue));
                    updatedEValues[row, col] = newValue == 0 ? 0 : newValue;
                }
            }

            double newRhsValue = row == pivotRowIndex ? rhsValues[row] / pivotIntersectionValue : rhsValues[row] - (xValues[row, pivotColumnIndex] * (rhsValues[pivotRowIndex] / pivotIntersectionValue));
            updatedRhsValues[row] = newRhsValue;
        }

        return (updatedXValues, updatedRhsValues, updatedSValues, updatedEValues);
    }

    public static int FindMostNegativeRHSIndex(double[] rhsValues)
    {
        double mostNegative = 0;
        int negativeIndex = -1;
        for (int i = 0; i < rhsValues.Length; i++)
        {
            double rhsValue = rhsValues[i];
            if (rhsValue < mostNegative)
            {
                mostNegative = rhsValue;
                negativeIndex = i;
            }
        }

        return negativeIndex;
    }

    public static int FindLeastPositiveThetaIndex(double[] thetaValues)
    {
        double leastPostive = double.PositiveInfinity;
        int positiveIndex = -1;
        for (int i = 0; i < thetaValues.Length; i++)
        {
            double thetaValue = thetaValues[i];
            if (thetaValue > 0 && thetaValue < leastPostive)
            {
                leastPostive = thetaValue;
                positiveIndex = i;
            }
        }

        return positiveIndex;
    }
}
