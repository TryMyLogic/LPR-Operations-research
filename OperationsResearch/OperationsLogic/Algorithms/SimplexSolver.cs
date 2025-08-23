using System.Text;

using OperationsLogic.Misc;

namespace OperationsLogic.Algorithms;

public class SimplexSolver : ISolver
{
    public void Solve(LinearModel model, out string output)
    {
        StringBuilder sb = new();
        _ = sb.AppendLine("Primal Simplex Iterations:");

        bool isMax = model.Type == "max";
        List<double> objCoeffs = [.. model.ObjectiveCoefficients];
        if (!isMax)
            for (int i = 0; i < objCoeffs.Count; i++)
                objCoeffs[i] = -objCoeffs[i];

        int n = objCoeffs.Count;
        int m = model.Constraints.Count;
        int totalVars = n + m;
        double[,] tableau = new double[m + 1, totalVars + 1];

        for (int i = 0; i < m; i++)
        {
            if (model.Constraints[i].Relation != "<=")
                _ = sb.AppendLine($"Warning: Constraint {i + 1} uses {model.Constraints[i].Relation}; only <= supported.");
            for (int j = 0; j < n; j++)
                tableau[i, j] = model.Constraints[i].Coefficients[j];
            tableau[i, n + i] = 1;
            tableau[i, totalVars] = model.Constraints[i].RHS;
        }

        for (int j = 0; j < n; j++)
            tableau[m, j] = isMax ? -objCoeffs[j] : objCoeffs[j];
        tableau[m, totalVars] = 0;

        int[] basis = new int[m];
        for (int i = 0; i < m; i++) basis[i] = n + i;

        _ = sb.AppendLine("Initial Tableau:");
        PrintTableau(tableau, sb);

        while (true)
        {
            int entering = -1;
            double minVal = 0;
            for (int j = 0; j < totalVars; j++)
                if (tableau[m, j] < minVal)
                {
                    minVal = tableau[m, j];
                    entering = j;
                }
            if (entering == -1) break;

            int leaving = -1;
            double minRatio = double.PositiveInfinity;
            for (int i = 0; i < m; i++)
                if (tableau[i, entering] > 0)
                {
                    double ratio = tableau[i, totalVars] / tableau[i, entering];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        leaving = i;
                    }
                }
            if (leaving == -1)
            {
                _ = sb.AppendLine("Problem is unbounded.");
                output = sb.ToString();
                return;
            }

            Misc.Pivot.Perform(tableau, leaving, entering);
            basis[leaving] = entering;

            _ = sb.AppendLine($"Iteration: Entering var x{entering + 1}, Leaving var x{basis[leaving] + 1}");
            PrintTableau(tableau, sb);
        }

        double objValue = isMax ? tableau[m, totalVars] : -tableau[m, totalVars]; // Correct sign based on problem type
        _ = sb.AppendLine($"Optimal Objective Value: {objValue:F3}");
        for (int j = 0; j < n; j++)
        {
            double val = 0;
            int row = Array.IndexOf(basis, j);
            if (row != -1) val = tableau[row, totalVars];
            _ = sb.AppendLine($"x{j + 1} = {val:F3}");
        }

        output = sb.ToString();
    }

    private void PrintTableau(double[,] tableau, StringBuilder sb)
    {
        int rows = tableau.GetLength(0);
        int cols = tableau.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                _ = sb.Append($"{tableau[i, j]:F3}\t");
            _ = sb.AppendLine();
        }
        _ = sb.AppendLine();
    }
}