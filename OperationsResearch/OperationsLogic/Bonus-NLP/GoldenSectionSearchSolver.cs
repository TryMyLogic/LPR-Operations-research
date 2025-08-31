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

        double x1 = a + resphi * (b - a);
        double x2 = b - resphi * (b - a);

        int iteration = 1;
        while (Math.Abs(b - a) > tolerance)
        {
            steps.Add($"Iteration {iteration}: Interval=({a:F4}, {b:F4}), x1={x1:F4}, x2={x2:F4}");

            if (problem.Objective(new double[] { x1 }) < problem.Objective(new double[] { x2 }))
            {
                b = x2;
                x2 = x1;
                x1 = a + resphi * (b - a);
            }
            else
            {
                a = x1;
                x1 = x2;
                x2 = b - resphi * (b - a);
            }

            iteration++;
        }

        double result = (a + b) / 2;
        steps.Add($"Final Result: x ≈ {result:F6}, f(x) = {problem.Objective(new double[] { result }):F6}");
        return result;
    }
}
