using System.Text;

using OperationsLogic.Misc;

namespace OperationsLogic.Algorithms;

public class RevisedSimplexSolver : ISolver
{
    public void Solve(LinearModel model, out string output)
    {
        StringBuilder sb = new();
        sb.AppendLine("==============================");
        sb.AppendLine("   Revised Primal Simplex");
        sb.AppendLine("==============================");

        bool isMax = model.Type == "max";
        List<double> objCoeffs = [.. model.ObjectiveCoefficients];

        if (!isMax)
            for (int i = 0; i < objCoeffs.Count; i++)
                objCoeffs[i] = -objCoeffs[i];

        int n = objCoeffs.Count;
        int m = model.Constraints.Count;
        int totalVars = n + m;

        double[,] A = new double[m, totalVars];
        double[] b = new double[m];
        double[] c = new double[totalVars];

        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
                A[i, j] = model.Constraints[i].Coefficients[j];
            A[i, n + i] = 1;
            b[i] = model.Constraints[i].RHS;
        }

        for (int j = 0; j < n; j++)
            c[j] = objCoeffs[j];

        int[] basis = new int[m];
        for (int i = 0; i < m; i++) basis[i] = n + i;

        double[,] BInv = new double[m, m];
        for (int i = 0; i < m; i++) BInv[i, i] = 1;

        double[] xB = MultiplyMatrixVector(BInv, b);

        sb.AppendLine("\nCanonical Form:");
        sb.AppendLine("------------------------------");
        PrintMatrix(A, b, c, sb, n, m, totalVars);
        sb.AppendLine();

        int iter = 0;
        while (true)
        {
            iter++;
            sb.AppendLine($"\nIteration {iter}");
            sb.AppendLine("------------------------------");

            double[] cB = GetCBasis(c, basis);
            double[] pi = MultiplyVectorMatrix(cB, BInv);

            sb.AppendLine($"Basis Variables: {string.Join(", ", basis.Select(bi => $"x{bi + 1}"))}");
            sb.AppendLine($"xB = [ {string.Join(", ", xB.Select(v => v.ToString("F3")))} ]");
            sb.AppendLine($"Dual Variables (pi) = [ {string.Join(", ", pi.Select(v => v.ToString("F3")))} ]");

            double bestReduced = 0;
            int entering = -1;
            for (int j = 0; j < totalVars; j++)
            {
                if (Array.IndexOf(basis, j) != -1) continue;
                double[] Aj = GetColumn(A, j);
                double reduced = c[j] - Dot(pi, Aj);
                sb.AppendLine($"Reduced cost for x{j + 1}: {reduced:F3}");
                if (reduced > bestReduced + 1e-9)
                {
                    bestReduced = reduced;
                    entering = j;
                }
            }

            if (entering == -1)
            {
                sb.AppendLine("--> Optimal solution reached.");
                break;
            }

            sb.AppendLine($"\n--> Entering variable: x{entering + 1}");

            double[] AEnter = GetColumn(A, entering);
            double[] d = MultiplyMatrixVector(BInv, AEnter);

            double minRatio = double.PositiveInfinity;
            int leaving = -1;
            for (int i = 0; i < m; i++)
            {
                if (d[i] > 1e-9)
                {
                    double ratio = xB[i] / d[i];
                    sb.AppendLine($"Ratio test: {xB[i]:F3} / {d[i]:F3} = {ratio:F3}");
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        leaving = i;
                    }
                }
            }

            if (leaving == -1)
            {
                sb.AppendLine("--> Problem is unbounded.");
                output = sb.ToString();
                return;
            }

            sb.AppendLine($"--> Leaving variable: x{basis[leaving] + 1}");

            double[] newxB = new double[m];
            for (int i = 0; i < m; i++)
                newxB[i] = xB[i] - d[i] * minRatio;
            newxB[leaving] = minRatio;
            xB = newxB;

            basis[leaving] = entering;

            double[,] B = new double[m, m];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < m; j++)
                    B[i, j] = A[i, basis[j]];
            BInv = InvertMatrix(B);

            sb.AppendLine("Updated Basis Inverse:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                    sb.Append($"{BInv[i, j]:F3}\t");
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        double objValue = Dot(GetCBasis(c, basis), xB);
        sb.AppendLine("==============================");
        sb.AppendLine($"Optimal Objective Value: {objValue:F3}");
        sb.AppendLine("Solution:");
        for (int j = 0; j < n; j++)
            sb.AppendLine($"x{j + 1} = {xB.ElementAtOrDefault(Array.IndexOf(basis, j)):F3}");
        sb.AppendLine("==============================");

        output = sb.ToString();
    }

    private double[] GetCBasis(double[] c, int[] basis)
    {
        double[] cb = new double[basis.Length];
        for (int i = 0; i < basis.Length; i++) cb[i] = c[basis[i]];
        return cb;
    }

    private double[] GetColumn(double[,] matrix, int col)
    {
        double[] column = new double[matrix.GetLength(0)];
        for (int i = 0; i < column.Length; i++) column[i] = matrix[i, col];
        return column;
    }

    private double Dot(double[] a, double[] b)
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++) sum += a[i] * b[i];
        return sum;
    }

    private double[] MultiplyVectorMatrix(double[] vec, double[,] mat)
    {
        double[] res = new double[mat.GetLength(1)];
        for (int j = 0; j < mat.GetLength(1); j++)
            for (int i = 0; i < vec.Length; i++)
                res[j] += vec[i] * mat[i, j];
        return res;
    }

    private double[] MultiplyMatrixVector(double[,] mat, double[] vec)
    {
        double[] res = new double[mat.GetLength(0)];
        for (int i = 0; i < mat.GetLength(0); i++)
            for (int k = 0; k < vec.Length; k++)
                res[i] += mat[i, k] * vec[k];
        return res;
    }

    private double[,] InvertMatrix(double[,] mat)
    {
        int n = mat.GetLength(0);
        double[,] res = new double[n, n];
        double[,] aug = new double[n, 2 * n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++) aug[i, j] = mat[i, j];
            aug[i, n + i] = 1;
        }

        for (int i = 0; i < n; i++)
        {
            double diag = aug[i, i];
            for (int j = 0; j < 2 * n; j++) aug[i, j] /= diag;
            for (int k = 0; k < n; k++)
            {
                if (k == i) continue;
                double factor = aug[k, i];
                for (int j = 0; j < 2 * n; j++) aug[k, j] -= factor * aug[i, j];
            }
        }

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                res[i, j] = aug[i, j + n];

        return res;
    }

    private void PrintMatrix(double[,] A, double[] b, double[] c, StringBuilder sb, int n, int m, int totalVars)
    {
        sb.AppendLine("Matrix A and RHS b:");
        for (int i = 0; i < m; i++)
        {
            sb.Append("| ");
            for (int j = 0; j < totalVars; j++)
                sb.Append($"{A[i, j]:F3}\t");
            sb.AppendLine($"| {b[i]:F3}");
        }
        sb.AppendLine("Objective Row c:");
        sb.Append("| ");
        for (int j = 0; j < totalVars; j++)
            sb.Append($"{c[j]:F3}\t");
        sb.AppendLine("|");
    }
}
