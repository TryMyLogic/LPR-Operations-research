using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using OperationsLogic.Algorithms;
using OperationsLogic.Model;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OperationsLogic;

interface IBranchAndBound
{
    int B_BDetermineTargetColumnIndex(double[,] xValues, int rowindex) { return -1; }
    void B_BProcess(CanonicalTableau tableau) { }
}
public class Branch_Bound : IBranchAndBound
{
    public List<CanonicalTableau> CompletedBranches { get; } = new();

    public int B_BDetermineBranchRowIndex(double[] rhsValues)
    {
        if (rhsValues == null || rhsValues.Length == 0)
            throw new ArgumentException("Array cannot be null or empty.");

        int index = -1;

        // skip row 0 intentionally
        for (int i = 1; i < rhsValues.Length; i++)
        {
            if ((int)rhsValues[i] != rhsValues[i]) // fractional value
            {
                double fractionalPart = rhsValues[i] % 1;
                double distance = Math.Abs(fractionalPart - 0.5);

                if (index == -1 ||
                    distance < Math.Abs((rhsValues[index] % 1) - 0.5))
                {
                    index = i;
                }
                else if (distance == Math.Abs((rhsValues[index] % 1) - 0.5))
                {
                    // tie-breaker
                    if (fractionalPart > (rhsValues[index] % 1))
                        index = i;
                }
            }
        }

        // if no fractionals found, index stays -1
        return index;
    }
        
    public int B_BDetermineTargetColumnIndex(double[,] xValues, int rowindex)
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
            if (columnmax == 1 && xValues[rowindex, i] == 1)
            {
                columnIndex = i; // Found the target column index
            }
        }
        return columnIndex; // If no target column is found, return -1

    }

    private double[] CreateNewRow(int length, int targetIndex,int val,int rhsval)
    {
        double[] row = new double[length +1];
        for (int i = 0; i < length+1; i++)
        {
            row[i] = 0; 
        }
        row[targetIndex] = 1;
        row[length] = val;
        row[length+1] = rhsval;
        return row;
    }

    private double[] SubtractTargetRowWithNewRow(double[] targetRow, double[] unitRow)
    {
        int len = Math.Min(targetRow.Length, unitRow.Length);
        for (int i = 0; i < len; i++)
            targetRow[i] -= unitRow[i];
        return targetRow;
    }
    private double[] AddNegativeToRow(double[] targetRow)
    {
        double[] newRow = new double[targetRow.Length];
        for (int i = 0; i < targetRow.Length; i++)
            newRow[i] = -targetRow[i];
        return newRow;
    }

    public CanonicalTableau B_BAddNewConstraint(CanonicalTableau tableu, string branch)
    {
        
        double[] rhsValues= new double[tableu.Rows];
        for (int i = 0; i < tableu.Rows; i++)
        {
            rhsValues[i] = tableu.Tableau[i, tableu.TotalVars];
        }
        int rowIndex = B_BDetermineBranchRowIndex(rhsValues);
        if (rowIndex == -1)
            throw new ArgumentException("No fractional values found in the array.");
        double[] targetRow = new double[tableu.TotalVars];

        for (int i = 0; i < tableu.TotalVars; i++)
        {
            targetRow[i] = tableu.Tableau[rowIndex, i];
        }
        double[,] xValues = new double[tableu.Rows, tableu.DecisionVars];
        for (int i = 0; i < tableu.Rows; i++)
        {
            for (int j = 0; j < tableu.DecisionVars; j++)
            {
                xValues[i, j] = tableu.Tableau[i, j];
            }
        }
        int columnIndex = B_BDetermineTargetColumnIndex(xValues, rowIndex);
        if (columnIndex == -1)
            throw new ArgumentException("No target column found for the given row index.");
        double nthRhsValue = rhsValues[rowIndex];

        if (branch == ">=")
        {
            nthRhsValue = Math.Ceiling(nthRhsValue);
        }
        else if (branch == "<=")
        {
            nthRhsValue = Math.Floor(nthRhsValue);
        }
        double[] newRow = new double[tableu.TotalVars +1];

        
        double[] updatedTargetRow = new double[tableu.TotalVars + 1];
        for (int i = 0; i < updatedTargetRow.Length; i++)
            {
                if (i != updatedTargetRow.Length)
                updatedTargetRow[i] = tableu.Tableau[rowIndex, i];
                else
                updatedTargetRow[i] = 0;
            }

        double newRhsValue = rhsValues[rowIndex] - nthRhsValue;
        
        switch (branch)
        {
            case "<=":
                newRow = CreateNewRow(tableu.TotalVars, columnIndex, 1, (int)nthRhsValue);
                break;
            case ">=":
                newRow = CreateNewRow(tableu.TotalVars, columnIndex, -1, (int)nthRhsValue);
                break;
        }
        if (updatedTargetRow.Length != newRow.Length)
            throw new ArgumentException("Rows must be of the same length to perform subtraction.");
        double[] nthXRow = SubtractTargetRowWithNewRow(updatedTargetRow, newRow);
        if (newRhsValue > 0)
        {
            nthXRow = AddNegativeToRow(nthXRow);
        }
    
        // Reconstruct the tableau
        CanonicalTableau finaltableu = new CanonicalTableau();
        finaltableu.IsMaximization = tableu.IsMaximization;
        finaltableu.DecisionVars = tableu.DecisionVars;
        if (branch == "<=")
        {
            finaltableu.SlackVars = tableu.SlackVars + 1;
        }
        else if (branch == ">=")
        {
            finaltableu.ExcessVars = tableu.ExcessVars + 1;
        }
        
        finaltableu.Rows = tableu.Rows + 1;


        for (int i = 0; i < finaltableu.Rows; i++)
        {
            finaltableu.Tableau[finaltableu.Rows, i] = nthXRow[i];//add new row
        }
        return finaltableu;


    }



    public void B_BProcess(CanonicalTableau tableau)
    {
        var (log, solvedTableau) = DualSimplex.Solve(tableau);

        double[] rhsValues = new double[solvedTableau.Rows];
        for (int i = 0; i < solvedTableau.Rows; i++)
        {
            rhsValues[i] = solvedTableau.Tableau[i, solvedTableau.TotalVars];
        }

        int branchRowIndex = B_BDetermineBranchRowIndex(rhsValues);


        //add displaytableau here if you want to see iterations
        



        if (branchRowIndex == -1)
        {
            CompletedBranches.Add(solvedTableau);
            return;
        }
        //floor 
        try
        {
            CanonicalTableau floorBranch = B_BAddNewConstraint(solvedTableau, "<=");
            B_BProcess(floorBranch); // recurse
        }
        catch
        {
            CompletedBranches.Add(solvedTableau);
        }

       //ceiling
        try
        {
            CanonicalTableau ceilBranch = B_BAddNewConstraint(solvedTableau, ">=");
            B_BProcess(ceilBranch); // recurse
        }
        catch
        {
            CompletedBranches.Add(solvedTableau);
        }
    }

    public void BestBranch(out string output, out CanonicalTableau bestTableau)
    {
        StringBuilder sb = new();
        double bestZ = double.NegativeInfinity;
        CanonicalTableau? best = null;
        foreach (var branch in CompletedBranches)
        {
            double zValue = branch.Tableau[0, branch.TotalVars];
            if (branch.IsMaximization)
            {
                if (zValue > bestZ)
                {
                    bestZ = zValue;
                    best = branch;
                }
            }
            else
            {
                if (zValue < bestZ || bestZ == double.NegativeInfinity)
                {
                    bestZ = zValue;
                    best = branch;
                }
            }
        }
        if (best != null)
        {
            sb.AppendLine("=== Best Branch ===");
            sb.AppendLine(best.DisplayTableau());
            sb.AppendLine($"Optimal Z: {Math.Round(bestZ, 3)}");
        }
        else
        {
            sb.AppendLine("No feasible branches found.");
        }
        output = sb.ToString();
        bestTableau = best ?? new CanonicalTableau();
    }


}
