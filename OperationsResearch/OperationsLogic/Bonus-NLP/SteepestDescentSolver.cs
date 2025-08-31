using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public class SteepestDescentSolver : INonLinearSolver
{
    private readonly double learningRate;
    private readonly double tolerance;
    private readonly int maxIterations;

    public SteepestDescentSolver(double learningRate = 0.01, double tolerance = 1e-6, int maxIterations = 1000)
    {
        this.learningRate = learningRate;
        this.tolerance = tolerance;
        this.maxIterations = maxIterations;
    }

    public double SolveWithSteps(NonLinearProblem problem, double[] initialGuess, List<string> steps)
    {
        double[] x = (double[])initialGuess.Clone();

        for (int iter = 0; iter < maxIterations; iter++)
        {
            var grad = problem.Gradient(x);
            double gradNorm = Math.Sqrt(grad.Select(g => g * g).Sum());

            steps.Add($"Iteration {iter + 1}: x=({string.Join(", ", x.Select(v => v.ToString("F4")))}), Gradient=({string.Join(", ", grad.Select(g => g.ToString("F4")))}), Norm={gradNorm:F6}");

            if (gradNorm < tolerance)
                break;

            for (int i = 0; i < x.Length; i++)
            {
                x[i] -= learningRate * grad[i]; // minimization
            }
        }

        double result = problem.Objective(x);
        steps.Add($"Final Result: x=({string.Join(", ", x.Select(v => v.ToString("F4")))}), f(x) = {result:F6}");
        return result;
    }
}
