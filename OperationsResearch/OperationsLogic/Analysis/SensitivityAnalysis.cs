using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperationsLogic.Misc;
using MathNet.Numerics.LinearAlgebra.Double;
using OperationsLogic.Model;

namespace OperationsLogic.Analysis;
public class SensitivityAnalysis
{
    private readonly LinearModel _linearModel;
    private readonly MathPreliminariesResult _result;
    public SensitivityAnalysis(LinearModel linearModel, MathPreliminariesResult result)
    {
        _linearModel = linearModel;
        _result = result;
    }
    public string ShowRangeNonBasic(int varIndex)
    {
        if (_result.BasicVariableIndices.Contains(varIndex))
            throw new InvalidOperationException($"x{varIndex + 1} is basic, not non-basic.");

        double reducedCost = _result.CjStar[varIndex];

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Sensitivity range for non-basic variable x{varIndex + 1}: ");
        sb.AppendLine($"ReducedCost: {Math.Round(reducedCost, 3)}");
        if (reducedCost > 0)
        {
            sb.AppendLine("Coefficient can decrease until reduced cost = 0");
        }
        else if (reducedCost < 0)
        {
            sb.AppendLine("Coefficient can increase until reduced cost = 0");
        }
        else
        {
            sb.AppendLine("Variable is at the threshold (multiple optimal solutions possible)");
        }

        return sb.ToString();
    }
    public string ApplyChangeNonBasic(int varIndex, double newCoeff)
    {
        if (_result.BasicVariableIndices.Contains(varIndex))
            throw new InvalidOperationException($"x{varIndex + 1} is basic, not non-basic.");

        double oldCoeff = _linearModel.ObjectiveCoefficients[varIndex];
        double delta = newCoeff - oldCoeff;
        double newReducedCost = _result.CjStar[varIndex] - delta;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Applied change to non-basic variable x{varIndex + 1}: ");
        sb.AppendLine($"Old coefficient: {oldCoeff}");
        sb.AppendLine($"New coefficient: {newCoeff}");
        sb.AppendLine($"New reduced cost: {Math.Round(newReducedCost, 3)}");
        sb.AppendLine(newReducedCost > 0 ? "Solution remains optimal." : "Solution may change - re-optimization required");

        return sb.ToString();
    }
    public string ShowRangeBasic(int varIndex)
    {
        if (!_result.BasicVariableIndices.Contains(varIndex))
            throw new InvalidOperationException($"{varIndex + 1} is non-basic, not basic.");

        int pos = _result.BasicVariableIndices.IndexOf(varIndex);
        double value = _result.BStar[pos];

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Sensitivity Range for basic variable x{varIndex + 1}: ");
        sb.AppendLine($"Current value: {Math.Round(value, 3)}");
        sb.AppendLine("Range determined by feasibility (value must remain >= 0).");

        return sb.ToString();
    }
    public string ApplyChangeBasic(int varIndex, double rhsDelta)
    {
        if (!_result.BasicVariableIndices.Contains(varIndex))
            throw new InvalidOperationException($"{varIndex + 1} is non-basic, not basic.");

        int pos = _result.BasicVariableIndices.IndexOf(varIndex);
        double oldValue = _result.BStar[pos];
        double newValue = oldValue + rhsDelta;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Applied change to Basic Variable x{varIndex + 1}: ");
        sb.AppendLine($"Old value: {Math.Round(oldValue, 3)}");
        sb.AppendLine($"RHS adjustment: {rhsDelta}");
        sb.AppendLine($"New value: {Math.Round(newValue, 3)}");
        sb.AppendLine(newValue >= 0 ? " Solution remains feasible." : " Solution is infeasible - re-optimization required.");

        return sb.ToString();
    }
}
