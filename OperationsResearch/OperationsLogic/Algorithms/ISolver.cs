using OperationsLogic.Misc;

namespace OperationsLogic.Algorithms;

public interface ISolver
{
    void Solve(LinearModel model, out string output);
}
