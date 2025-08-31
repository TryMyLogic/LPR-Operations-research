using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OperationsLogic.Misc;

namespace OperationsLogic.Algorithms
{
    public class SimplexSolver : ISolver
    {
        public double[,] FinalTableau { get; private set; } = new double[0, 0];
        public int NumDecisionVars { get; private set; } = 0;
        public int NumSlackVars { get; private set; } = 0;
        public int NumExcessVars { get; private set; } = 0;

        // NEW: store the basis column indices (length = m)
        public int[] BasisIndices { get; private set; } = Array.Empty<int>();

        // NEW: store the initial constraint matrix columns (m x totalVars) before pivots
        public double[,] InitialConstraintMatrix { get; private set; } = new double[0, 0];

        // NEW: original objective coefficients as given in model (not modified for max/min)
        public List<double> OriginalObjectiveCoeffs { get; private set; } = new List<double>();

        public void Solve(LinearModel model, out string output)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("==============================");
            sb.AppendLine("Simplex Iterations:");
            sb.AppendLine("==============================");

            bool isMax = model.Type == "max";
            List<double> objCoeffs = new List<double>(model.ObjectiveCoefficients);
            OriginalObjectiveCoeffs = new List<double>(model.ObjectiveCoefficients);

            if (!isMax)
                for (int i = 0; i < objCoeffs.Count; i++)
                    objCoeffs[i] = -objCoeffs[i];

            int n = objCoeffs.Count;
            int m = model.Constraints.Count;

            NumDecisionVars = n;
            NumSlackVars = 0;
            NumExcessVars = 0;

            int totalVars = n + m;
            double[,] tableau = new double[m + 1, totalVars + 1];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                    tableau[i, j] = model.Constraints[i].Coefficients[j];

                if (model.Constraints[i].Relation == "<=")
                {
                    tableau[i, n + i] = 1;
                    NumSlackVars++;
                }
                else if (model.Constraints[i].Relation == ">=")
                {
                    tableau[i, n + i] = -1;
                    NumExcessVars++;
                }

                tableau[i, totalVars] = model.Constraints[i].RHS;
            }

            for (int j = 0; j < n; j++)
                tableau[m, j] = isMax ? -objCoeffs[j] : objCoeffs[j];

            tableau[m, totalVars] = 0;

            int[] basis = new int[m];
            for (int i = 0; i < m; i++)
                basis[i] = n + i;

            // Save initial constraint matrix (m x totalVars)
            InitialConstraintMatrix = new double[m, totalVars];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < totalVars; j++)
                    InitialConstraintMatrix[i, j] = tableau[i, j];

            sb.AppendLine("Initial Tableau:");
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
                {
                    if (tableau[i, entering] > 0)
                    {
                        double ratio = tableau[i, totalVars] / tableau[i, entering];
                        if (ratio < minRatio)
                        {
                            minRatio = ratio;
                            leaving = i;
                        }
                    }
                }

                if (leaving == -1)
                {
                    sb.AppendLine("Problem is unbounded.");
                    output = sb.ToString();
                    FinalTableau = tableau;
                    BasisIndices = basis;
                    return;
                }

                // perform pivot
                Pivot.Perform(tableau, leaving, entering);
                basis[leaving] = entering;

                sb.AppendLine($"Iteration: Entering var x{entering + 1}, Leaving var x{basis[leaving] + 1}");
                PrintTableau(tableau, sb);
            }

            double objValue = isMax ? tableau[m, totalVars] : -tableau[m, totalVars];
            sb.AppendLine($"Optimal Objective Value: {objValue:F3}");

            for (int j = 0; j < n; j++)
            {
                double val = 0;
                int row = Array.IndexOf(basis, j);
                if (row != -1) val = tableau[row, totalVars];
                sb.AppendLine($"x{j + 1} = {val:F3}");
            }

            FinalTableau = tableau;
            BasisIndices = basis;
            output = sb.ToString();
        }

        private void PrintTableau(double[,] tableau, StringBuilder sb)
        {
            int rows = tableau.GetLength(0);
            int cols = tableau.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    sb.Append($"{tableau[i, j]:F3}\t");
                sb.AppendLine();
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Compute simplex dual variables y by y^T = c_B^T * B^{-1}
        /// Returns null if unable to compute (missing basis/initial A or singular B).
        /// </summary>
        public double[]? GetDualVariables()
        {
            if (InitialConstraintMatrix == null || InitialConstraintMatrix.Length == 0) return null;
            if (BasisIndices == null || BasisIndices.Length == 0) return null;

            int m = InitialConstraintMatrix.GetLength(0);
            int totalVars = InitialConstraintMatrix.GetLength(1);
            if (BasisIndices.Length != m) return null;

            // Build B from columns corresponding to basis indices
            double[,] B = new double[m, m];
            for (int col = 0; col < m; col++)
            {
                int varIndex = BasisIndices[col];
                if (varIndex < 0 || varIndex >= totalVars) return null;
                for (int row = 0; row < m; row++)
                    B[row, col] = InitialConstraintMatrix[row, varIndex];
            }

            // c_B: objective coefficients for basic variables (if basic var < NumDecisionVars take from OriginalObjectiveCoeffs)
            double[] cB = new double[m];
            for (int i = 0; i < m; i++)
            {
                int varIdx = BasisIndices[i];
                if (varIdx < NumDecisionVars && varIdx < OriginalObjectiveCoeffs.Count)
                    cB[i] = OriginalObjectiveCoeffs[varIdx];
                else
                    cB[i] = 0.0; // slack/excess have 0 objective coeff
            }

            var invB = InvertMatrix(B);
            if (invB == null) return null;

            // y^T = c_B^T * B^{-1} -> compute y vector (length m)
            double[] y = new double[m];
            for (int j = 0; j < m; j++)
            {
                double sum = 0;
                for (int i = 0; i < m; i++)
                    sum += cB[i] * invB[i, j];
                y[j] = sum;
            }

            return y;
        }

        // Gauss-Jordan inverse for small matrices
        private static double[,]? InvertMatrix(double[,] A)
        {
            int n = A.GetLength(0);
            if (n != A.GetLength(1)) return null;
            double[,] aug = new double[n, 2 * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) aug[i, j] = A[i, j];
                aug[i, n + i] = 1.0;
            }

            for (int col = 0; col < n; col++)
            {
                int pivotRow = col;
                double maxAbs = Math.Abs(aug[pivotRow, col]);
                for (int r = col + 1; r < n; r++)
                {
                    double val = Math.Abs(aug[r, col]);
                    if (val > maxAbs) { maxAbs = val; pivotRow = r; }
                }
                if (Math.Abs(aug[pivotRow, col]) < 1e-12) return null;

                if (pivotRow != col)
                {
                    for (int c = 0; c < 2 * n; c++)
                    {
                        double tmp = aug[col, c];
                        aug[col, c] = aug[pivotRow, c];
                        aug[pivotRow, c] = tmp;
                    }
                }

                double pivot = aug[col, col];
                for (int c = 0; c < 2 * n; c++) aug[col, c] /= pivot;

                for (int r = 0; r < n; r++)
                {
                    if (r == col) continue;
                    double factor = aug[r, col];
                    if (Math.Abs(factor) < 1e-15) continue;
                    for (int c = 0; c < 2 * n; c++) aug[r, c] -= factor * aug[col, c];
                }
            }

            double[,] inv = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    inv[i, j] = aug[i, n + j];

            return inv;
        }
    }

    public class DualitySolver
    {
        private readonly SimplexSolver _primalSolver = new();

        /// <summary>
        /// Solve primal (with SimplexSolver) then extract dual variables y = c_B^T B^{-1}.
        /// Compute dual objective b^T y and compare to primal objective.
        /// </summary>
        public void TestDuality(LinearModel primal, out string output)
        {
            var sb = new StringBuilder();
            sb.AppendLine("==================================");
            sb.AppendLine("PRIMAL PROBLEM (from file)");
            sb.AppendLine("==================================");
            sb.AppendLine(FormatPrimal(primal));
            sb.AppendLine();

            _primalSolver.Solve(primal, out string primalOut);
            sb.AppendLine(primalOut);

            if (_primalSolver.FinalTableau == null || _primalSolver.FinalTableau.Length == 0)
            {
                sb.AppendLine("Primal solve failed: empty final tableau. Cannot compute dual.");
                output = sb.ToString();
                return;
            }

            double[]? y = _primalSolver.GetDualVariables();
            if (y == null)
            {
                sb.AppendLine("Could not compute dual variables (singular B or missing basis).");
                output = sb.ToString();
                return;
            }

            sb.AppendLine("=== Dual variables (computed from primal) ===");
            for (int i = 0; i < y.Length; i++)
                sb.AppendLine($"y{i + 1} = {y[i]:F6}");

            // dual objective b^T y
            double dualVal = 0;
            for (int i = 0; i < primal.Constraints.Count; i++)
                dualVal += primal.Constraints[i].RHS * y[i];

            // read primal objective from the primal final tableau
            int prRows = _primalSolver.FinalTableau.GetLength(0);
            int prCols = _primalSolver.FinalTableau.GetLength(1);
            double primalObj = primal.Type == "max"
                ? _primalSolver.FinalTableau[prRows - 1, prCols - 1]
                : -_primalSolver.FinalTableau[prRows - 1, prCols - 1];

            sb.AppendLine();
            sb.AppendLine("==================================");
            sb.AppendLine("DUALITY CHECK");
            sb.AppendLine("==================================");
            sb.AppendLine($"Primal objective (from tableau): {primalObj:F6}");
            sb.AppendLine($"Dual objective:           {dualVal:F6}");
            sb.AppendLine($"Duality Difference: {dualVal - primalObj}");

            if (Math.Abs(primalObj - dualVal) <= 1e-6)
                sb.AppendLine("Strong duality holds (primal == dual).");
            else
                sb.AppendLine("Values differ — check model form or numerical issues.");

            output = sb.ToString();
        }

        private static string FormatPrimal(LinearModel model)
        {
            var sb = new StringBuilder();
            sb.Append(model.Type.Equals("max", StringComparison.OrdinalIgnoreCase) ? "Max  z = " : "Min  z = ");
            for (int j = 0; j < model.ObjectiveCoefficients.Count; j++)
            {
                double c = model.ObjectiveCoefficients[j];
                if (j > 0) sb.Append(" + ");
                sb.Append($"{c} x{j + 1}");
            }
            sb.AppendLine();
            sb.AppendLine("s.t.");
            for (int i = 0; i < model.Constraints.Count; i++)
            {
                var ct = model.Constraints[i];
                for (int j = 0; j < ct.Coefficients.Count; j++)
                {
                    if (j > 0) sb.Append(" + ");
                    sb.Append($"{ct.Coefficients[j]} x{j + 1}");
                }
                sb.AppendLine($"  {ct.Relation}  {ct.RHS}");
            }
            return sb.ToString();
        }
    }
}
