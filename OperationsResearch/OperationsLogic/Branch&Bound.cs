using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OperationsLogic;

public interface IBranchAndBoundSettings
{
    void B_BProcess(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null) { }
}
public class BranchAndBoundSettings : IBranchAndBoundSettings
{
    void B_BProcess(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null) { }
}
public class Branch_Bound : IBranchAndBoundSettings
{

    public int B_BDetermineBranchRowIndex(double[] rhsValues)
    {   
        int fractionalCount = 0;
        for (int i = 1; i < rhsValues.Length; i++)
        {
            if ((int)rhsValues[i] == rhsValues[i])
            {
                fractionalCount++;
            }
        }
        if (fractionalCount > 0)
        {
            if (rhsValues == null || rhsValues.Length == 0)
                throw new ArgumentException("Array cannot be null or empty.");
            if (rhsValues == null || rhsValues.Length <= 1)
                throw new ArgumentException("Array must have at least 2 elements to exclude the first.");
            int index = 0;
            double closestValue = rhsValues[1];
            double smallestDistance = Math.Abs((rhsValues[1] % 1) - 0.5);

            for (int i = 2; i < rhsValues.Length; i++)
            {
                double fractionalPart = rhsValues[i] % 1;
                if (fractionalPart < 0) fractionalPart += 1; // handle negatives

                double distance = Math.Abs(fractionalPart - 0.5);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestValue = rhsValues[i];
                    index = i;
                }
            }

            return index;
        }
        else
        {
            throw new ArgumentException("No fractional values found in the array.");
        }
    }
    public int B_BDetermineTargetColumnIndex(double[,] xValues,int rowindex)
    {
        int columnIndex = -1;
        //searching for the target column index of the column with the basic variable that correlates to the row index provided by B_BDetermineBranchRowIndex
        for (int i = 0; i < xValues.GetLength(1); i++)
        {
            double columnmax = 0;
            for (int j = 0; j < xValues.GetLength(0); j++)
            {
                columnmax += xValues[j, i]; 
            }
            if (columnmax == 1 && xValues[rowindex,i] == 1)
            {
               columnIndex = i; // Found the target column index
            }
        }
        return columnIndex; // If no target column is found, return -1

    }
    private double[] ExtractRow(double[,] matrix, int rowIndex)
    {
        int cols = matrix.GetLength(1);
        double[] row = new double[cols];
        for (int i = 0; i < cols; i++)
            row[i] = matrix[rowIndex, i];
        return row;
    }

    private double[] CreateNewRow(int length, int targetIndex)
    {
        double[] row = new double[length];
        if (targetIndex >= 0 && targetIndex < length)
            row[targetIndex] = 1;
        return row;
    }

    private void SubtractTargetRowWithNewRow(double[] targetRow, double[] unitRow)
    {
        int len = Math.Min(targetRow.Length, unitRow.Length);
        for (int i = 0; i < len; i++)
            targetRow[i] -= unitRow[i];
    }
    private double[] AddNegativeToRow(double[] targetRow)
    {
        double[] newRow = new double[targetRow.Length];
        for (int i = 0; i < targetRow.Length; i++)
            newRow[i] = -targetRow[i];
        return newRow;
    }

    private void AddRowToMatrix(ref double[,] matrix, double[] newRow)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        if (newRow.Length != cols)
            throw new ArgumentException("New row length must match matrix column count.");
        double[,] newMatrix = new double[rows + 1, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                newMatrix[i, j] = matrix[i, j];
        for (int j = 0; j < cols; j++)
            newMatrix[rows, j] = newRow[j];
        matrix = newMatrix;
    }

    public (double[,] xValues, double[] rhsValues, double[,] sValues, double[,] eValues) B_BAddNewConstraint(double[,] xValues, double[] rhsValues, string branch,
                                    double[,]? sValues = null, double[,]? eValues = null)
    {
        int rowIndex = B_BDetermineBranchRowIndex(rhsValues);
        int columnIndex = B_BDetermineTargetColumnIndex(xValues, rowIndex);

       
        double[] newXValues = ExtractRow(xValues, rowIndex);
        double[] newSValues = sValues != null ? ExtractRow(sValues, rowIndex) : Array.Empty<double>();
        double[] newEValues = eValues != null ? ExtractRow(eValues, rowIndex) : Array.Empty<double>();

      
        double[] nthXRow = CreateNewRow(xValues.GetLength(1), columnIndex);
        double[] nthSRow = sValues != null ? CreateNewRow(sValues.GetLength(1), sValues.GetLength(1) - 1) : Array.Empty<double>();
        double[] nthERow = eValues != null ? CreateNewRow(eValues.GetLength(1), eValues.GetLength(1) - 1) : Array.Empty<double>();

        
        SubtractTargetRowWithNewRow(newXValues, nthXRow);
        SubtractTargetRowWithNewRow(newSValues, nthSRow);
        SubtractTargetRowWithNewRow(newEValues, nthERow);
        
        double nthRhsValue = rhsValues[rowIndex];
        if (branch == "<=")
            nthRhsValue = Math.Floor(rhsValues[rowIndex]);
        else if (branch == ">=")
            nthRhsValue = Math.Ceiling(rhsValues[rowIndex]);
        else
            throw new ArgumentException("Branch must be either '<=' or '>='.");
        double newRhsValue = rhsValues[rowIndex] - nthRhsValue;
        if (newRhsValue > 0)
        {
            nthXRow = AddNegativeToRow(nthXRow);
            nthSRow = AddNegativeToRow(nthSRow);
            nthERow = AddNegativeToRow(nthERow);
            nthRhsValue = -nthRhsValue;
        }
        // Now, newXValues, newSValues, newEValues, and newRhsValue contain the updated values for the specified row which care now added to the matrices
        AddRowToMatrix(ref xValues, nthXRow);
        if (sValues != null)
            AddRowToMatrix(ref sValues, nthSRow);
        if (eValues != null)
            AddRowToMatrix(ref eValues, nthERow);
        double[] newRhsArray = new double[rhsValues.Length + 1];
        Array.Copy(rhsValues, newRhsArray, rhsValues.Length);
        newRhsArray[newRhsArray.Length - 1] = nthRhsValue;
        rhsValues = newRhsArray;
        return(xValues, rhsValues, sValues ?? new double[0, 0], eValues ?? new double[0, 0]);

    }



    public void B_BProcess(double[,] xValues, double[] rhsValues, double[,]? sValues = null, double[,]? eValues = null)
    {


    }

    public void B_BDataSend()
    {
    }
}
