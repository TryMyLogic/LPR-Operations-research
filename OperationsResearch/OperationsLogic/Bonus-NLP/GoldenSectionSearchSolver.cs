using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public class GoldenSectionSearchSolver : INonLinearSolver
{
    private readonly double tolerance;
    private readonly double lowerBound;
    private readonly double upperBound;

    public GoldenSectionSearchSolver(double lowerBound, double upperBound, double tolerance = 1e-5)
    {
        this.lowerBound = lowerBound;
        this.upperBound = upperBound;
        this.tolerance = tolerance;
    }

    public double SolveWithSteps(NonLinearProblem problem, double[] initialGuess, List<string> steps)
    {
        double a = lowerBound, b = upperBound;
        double phi = (1 + Math.Sqrt(5)) / 2;
        double resphi = 2 - phi;

        // Initial internal points
        double x1 = a + resphi * (b - a);
        double x2 = b - resphi * (b - a);

        // Cache function evaluations
        double f1 = problem.Objective(new double[] { x1 });
        double f2 = problem.Objective(new double[] { x2 });

        int iteration = 1;

        while (Math.Abs(b - a) > tolerance)
        {
            steps.Add(
                $"Iteration {iteration}: Interval=({a:F6}, {b:F6}), " +
                $"x1={x1:F6}, f1={f1:F6}, x2={x2:F6}, f2={f2:F6}"
            );

            if (f1 < f2)
            {
                b = x2;
                x2 = x1; f2 = f1; // Reuse f1
                x1 = a + resphi * (b - a);
                f1 = problem.Objective(new double[] { x1 });
            }
            else
            {
                a = x1;
                x1 = x2; f1 = f2; // Reuse f2
                x2 = b - resphi * (b - a);
                f2 = problem.Objective(new double[] { x2 });
            }

            iteration++;
        }

        // Pick the best point found
        double result = (f1 < f2) ? x1 : x2;
        double fResult = problem.Objective(new double[] { result });

        steps.Add($"Final Result: x ≈ {result:F6}, f(x) = {fResult:F6}");

        return result;
    }
}
