using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Windows_Project.Algorithms
{
    public class RevisedSimplexSolver : ISolver
    {
        public void Solve(LinearModel model, out string output)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Revised Primal Simplex Iterations:");

            bool isMax = model.Type == "max";
            List<double> objCoeffs = new List<double>(model.ObjectiveCoefficients);
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
                c[j] = isMax ? objCoeffs[j] : -objCoeffs[j];

            int[] basis = new int[m];
            for (int i = 0; i < m; i++) basis[i] = n + i;

            double[,] BInv = new double[m, m];
            for (int i = 0; i < m; i++) BInv[i, i] = 1;

            double[] xB = MultiplyMatrixVector(BInv, b);

            int iter = 0;
            while (true)
            {
                iter++;
                sb.AppendLine($"Iteration {iter}:");

                double[] cB = GetCBasis(c, basis);
                double[] pi = MultiplyVectorMatrix(cB, BInv); // Dual variables

                double maxReduced = 0;
                int entering = -1;
                for (int j = 0; j < totalVars; j++)
                {
                    if (Array.IndexOf(basis, j) != -1) continue;
                    double[] Aj = GetColumn(A, j);
                    double reduced = c[j] - Dot(pi, Aj);
                    if (reduced < maxReduced)
                    {
                        maxReduced = reduced;
                        entering = j;
                    }
                }
                if (entering == -1)
                {
                    sb.AppendLine("Optimal reached.");
                    break;
                }

                sb.AppendLine($"Reduced Costs: c[{entering}] = {c[entering]}, pi . A[{entering}] = {Dot(pi, GetColumn(A, entering))}, Reduced = {maxReduced}");
                double[] AEnter = GetColumn(A, entering);
                double[] col = MultiplyMatrixVector(BInv, AEnter);

                double minRatio = double.PositiveInfinity;
                int leaving = -1;
                for (int i = 0; i < m; i++)
                    if (col[i] > 0)
                    {
                        double ratio = xB[i] / col[i];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            leaving = i;
                        }
                    }
                if (leaving == -1)
                {
                    sb.AppendLine("Problem is unbounded.");
                    output = sb.ToString();
                    return;
                }

                double[,] eta = new double[m, m];
                for (int i = 0; i < m; i++) eta[i, i] = 1;
                double pivot = col[leaving];
                for (int i = 0; i < m; i++)
                    if (i == leaving) eta[i, leaving] = 1 / pivot;
                    else eta[i, leaving] = -col[i] / pivot;

                BInv = MultiplyMatrices(eta, BInv);
                xB = MultiplyMatrixVector(eta, xB);
                basis[leaving] = entering;

                sb.AppendLine("B^{-1} Update:");
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < m; j++)
                        sb.Append($"{BInv[i, j]:F3}\t");
                    sb.AppendLine();
                }
            }

            double objValue = Dot(GetCBasis(c, basis), xB);
            sb.AppendLine($"Optimal Objective Value: {objValue:F3}");
            double[] solution = new double[n];
            for (int i = 0; i < m; i++)
                if (basis[i] < n) solution[basis[i]] = xB[i];
            for (int j = 0; j < n; j++)
                sb.AppendLine($"x{j + 1} = {solution[j]:F3}");

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
                    res[j] += vec[i] * mat[i, j]; // Fixed row-major order
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

        private double[,] MultiplyMatrices(double[,] a, double[,] b)
        {
            double[,] res = new double[a.GetLength(0), b.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < b.GetLength(1); j++)
                    for (int k = 0; k < a.GetLength(1); k++)
                        res[i, j] += a[i, k] * b[k, j];
            return res;
        }
    }
}