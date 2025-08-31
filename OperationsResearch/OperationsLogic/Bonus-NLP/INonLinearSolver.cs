using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public interface INonLinearSolver
{
    double SolveWithSteps(NonLinearProblem problem, double[] initialGuess, List<string> steps);
}
