namespace OperationsLogic;

public class SimplexUtils
{
    public enum ColumnGroup
    {
        X,
        S,
        E
    }

    public static void DisplayTable(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null, double[,]? thetaValues = null)
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
        if (thetaValues != null)
        {
            Console.Write("Theta\t");
            for (int i = 0; i < xColumnCount; i++)
            {
                Console.Write($"{Math.Round(thetaValues[0, i], 3)}\t");
            }
            for (int i = 0; i < eColumnCount; i++)
            {
                Console.Write($"{Math.Round(thetaValues[1, i], 3)}\t");
            }
            for (int i = 0; i < sColumnCount; i++)
            {
                Console.Write($"{Math.Round(thetaValues[2, i], 3)}\t");
            }
            Console.WriteLine();
        }
    }

    public static (double[,] xValues, double[] rhsValues, double[,]? sValues, double[,]? eValues) PerformPivot(int pivotRowIndex, int pivotColumnIndex, double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null, ColumnGroup columnGroup = ColumnGroup.X)
    {
        double[,] updatedXValues = new double[xValues.GetLength(0), xValues.GetLength(1)];
        double[] updatedRhsValues = new double[rhsValues.Length];
        double[,]? updatedSValues = sValues != null ? new double[sValues.GetLength(0), sValues.GetLength(1)] : null;
        double[,]? updatedEValues = eValues != null ? new double[eValues.GetLength(0), eValues.GetLength(1)] : null;

        double pivotIntersectionValue = columnGroup switch
        {
            ColumnGroup.X => xValues[pivotRowIndex, pivotColumnIndex],
            ColumnGroup.S => sValues![pivotRowIndex, pivotColumnIndex],
            ColumnGroup.E => eValues![pivotRowIndex, pivotColumnIndex],
            _ => throw new ArgumentOutOfRangeException(nameof(columnGroup), columnGroup, "No such column group exists")
        };

        // Prevents NAN and an infinite loop in cases of negative infinity values as well
        if (pivotIntersectionValue == 0)
        {
            throw new InvalidDataException("The pivot intersection value is 0. You cannot divide by 0");
        }

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
                    double pivotColumnValue = columnGroup switch
                    {
                        ColumnGroup.X => xValues[row, pivotColumnIndex],
                        ColumnGroup.S => sValues![row, pivotColumnIndex],
                        ColumnGroup.E => eValues![row, pivotColumnIndex],
                        _ => throw new ArgumentOutOfRangeException(nameof(columnGroup), columnGroup, "No such column group exists")
                    };

                    // (xValues[pivotRowIndex, col] / pivotIntersectionValue) is the same as if row == pivotRowIndex newValue and done this way to avoid circuler reference
                    newValue = xValues[row, col] - (pivotColumnValue * (xValues[pivotRowIndex, col] / pivotIntersectionValue));
                }

                // The additional check fixes issues where negative zero and positive zero appear and make them zero
                updatedXValues[row, col] = newValue == 0 ? 0 : Math.Round(newValue, 3);
            }

            if (sValues != null && updatedSValues != null)
            {
                for (int col = 0; col < sValues.GetLength(1); col++)
                {
                    // Every newValue from here onwards uses ternary for conciseness. Above is left as is as its easier to read and understand for some. 
                    double pivotColumnValue = columnGroup switch
                    {
                        ColumnGroup.X => xValues[row, pivotColumnIndex],
                        ColumnGroup.S => sValues[row, pivotColumnIndex],
                        ColumnGroup.E => eValues![row, pivotColumnIndex],
                        _ => throw new ArgumentOutOfRangeException(nameof(columnGroup), columnGroup, "No such column group exists")
                    };
                    double newValue = row == pivotRowIndex ? sValues[row, col] / pivotIntersectionValue : sValues[row, col] - (pivotColumnValue * (sValues[pivotRowIndex, col] / pivotIntersectionValue));
                    updatedSValues[row, col] = newValue == 0 ? 0 : Math.Round(newValue, 3);
                }
            }

            if (eValues != null && updatedEValues != null)
            {
                for (int col = 0; col < eValues.GetLength(1); col++)
                {
                    double pivotColumnValue = columnGroup switch
                    {
                        ColumnGroup.X => xValues[row, pivotColumnIndex],
                        ColumnGroup.S => sValues![row, pivotColumnIndex],
                        ColumnGroup.E => eValues[row, pivotColumnIndex],
                        _ => throw new ArgumentOutOfRangeException(nameof(columnGroup), columnGroup, "No such column group exists")
                    };
                    double newValue = row == pivotRowIndex ? eValues[row, col] / pivotIntersectionValue : eValues[row, col] - (pivotColumnValue * (eValues[pivotRowIndex, col] / pivotIntersectionValue));
                    updatedEValues[row, col] = newValue == 0 ? 0 : Math.Round(newValue, 3);
                }
            }

            double pivotColumnValueRhs = columnGroup switch
            {
                ColumnGroup.X => xValues[row, pivotColumnIndex],
                ColumnGroup.S => sValues![row, pivotColumnIndex],
                ColumnGroup.E => eValues![row, pivotColumnIndex],
                _ => throw new ArgumentOutOfRangeException(nameof(columnGroup), columnGroup, "No such column group exists")
            };
            double newRhsValue = row == pivotRowIndex ? rhsValues[row] / pivotIntersectionValue : rhsValues[row] - (pivotColumnValueRhs * (rhsValues[pivotRowIndex] / pivotIntersectionValue));
            updatedRhsValues[row] = Math.Round(newRhsValue, 3);
        }

        return (updatedXValues, updatedRhsValues, updatedSValues, updatedEValues);
    }

    public static int FindMostNegativeRHSIndex(double[] rhsValues)
    {
        double mostNegative = 0;
        int negativeIndex = -1;
        for (int i = 1; i < rhsValues.Length; i++) // Starts at 1 since you dont check Z row RHS (always 0)
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

    public static (int pivotColumnIndex, SimplexUtils.ColumnGroup columnGroup) FindLeastPositiveThetaIndex(double[,] thetaValues, int xColumnCount, int sColumnCount, int eColumnCount)
    {
        double leastPositive = double.PositiveInfinity;
        int pivotColumnIndex = -1;
        SimplexUtils.ColumnGroup columnGroup = SimplexUtils.ColumnGroup.X;

        for (int col = 0; col < xColumnCount; col++)
        {
            double thetaValue = thetaValues[0, col];
            if (thetaValue > 0 && thetaValue < leastPositive)
            {
                leastPositive = thetaValue;
                pivotColumnIndex = col;
                columnGroup = SimplexUtils.ColumnGroup.X;
            }
        }

        for (int col = 0; col < sColumnCount; col++)
        {
            double thetaValue = thetaValues[1, col];
            if (thetaValue > 0 && thetaValue < leastPositive)
            {
                leastPositive = thetaValue;
                pivotColumnIndex = col;
                columnGroup = SimplexUtils.ColumnGroup.S;
            }
        }

        for (int col = 0; col < eColumnCount; col++)
        {
            double thetaValue = thetaValues[2, col];
            if (thetaValue > 0 && thetaValue < leastPositive)
            {
                leastPositive = thetaValue;
                pivotColumnIndex = col;
                columnGroup = SimplexUtils.ColumnGroup.E;
            }
        }

        return (pivotColumnIndex, columnGroup);
    }
}
