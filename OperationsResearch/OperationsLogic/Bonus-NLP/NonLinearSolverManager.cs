using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public class NonLinearSolverManager
{
    private NonLinearProblem _problem;

    // Parameterless constructor
    public NonLinearSolverManager() { }

    // Set the problem later
    public void SetProblem(NonLinearProblem problem)
    {
        _problem = problem;
    }

    public SolverResult Solve(double[] initialGuess)
    {
        if (_problem == null)
            throw new Exception("No problem set. Call SetProblem() first.");

        SolverResult result = new SolverResult();

        if (initialGuess.Length == 1) // 1D problem
        {
            var solver = new GoldenSectionSearchSolver(-10, 10);
            result.AlgorithmUsed = "Golden Section Search";
            result.FinalValue = solver.SolveWithSteps(_problem, initialGuess, result.Steps);
        }
        else // Multi-D problem
        {
            var solver = new SteepestDescentSolver(0.1);
            result.AlgorithmUsed = "Steepest Descent";
            result.FinalValue = solver.SolveWithSteps(_problem, initialGuess, result.Steps);
        }

        return result;
    }
}