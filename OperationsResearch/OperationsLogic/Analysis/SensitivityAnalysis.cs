using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

using OperationsLogic.Misc;
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
    // 5. 
    public string DescribeRHSRange(int constraintIndex)
    {
        var (min, max) = GetRHSRange(constraintIndex);
        if (double.IsNegativeInfinity(min) && double.IsPositiveInfinity(max))
            return $"Constraint {constraintIndex}: RHS can vary without limit.";
        if (double.IsNegativeInfinity(min))
            return $"Constraint {constraintIndex}: RHS can increase up to {max:F4}, no lower bound.";
        if (double.IsPositiveInfinity(max))
            return $"Constraint {constraintIndex}: RHS can decrease down to {min:F4}, no upper bound.";
        return $"Constraint {constraintIndex}: RHS can vary between {min:F4} and {max:F4}.";
    }

    private (double min, double max) GetRHSRange(int constraintIndex)
    {
        double[] column = new double[_result.BInverse.GetLength(0)];
        for (int i = 0; i < column.Length; i++)
            column[i] = _result.BInverse[i, constraintIndex];

        double min = double.NegativeInfinity;
        double max = double.PositiveInfinity;

        for (int i = 0; i < _result.BStar.Length; i++)
        {
            if (column[i] > 0)
                max = Math.Min(max, _result.BStar[i] / column[i]);
            else if (column[i] < 0)
                min = Math.Max(min, _result.BStar[i] / column[i]);
        }
        return (min, max);
    }

    // 6.
    public string ApplyRHSChange(int constraintIndex, double delta)
    {
        _result.BStar[constraintIndex] += delta;
        // Update Z*
        _result.ZStar = 0;
        for (int i = 0; i < _result.CBV.Length; i++)
            _result.ZStar += _result.CBV[i] * _result.BStar[i];
        return $"Applied change of {delta:+0.####;-0.####;0} to RHS of constraint {constraintIndex}. " +
               $"New RHS = {_result.BStar[constraintIndex]:F4}, new Z* = {_result.ZStar:F4}.";
    }

    // 7. 
    public string NonBasicVariableRange(int varIndex, double originalCj)
    {
        double reducedCost = _result.CjStar[varIndex];
        if (reducedCost > 0)
            return $"Variable x{varIndex} (nonbasic): cost coefficient can decrease to {originalCj - reducedCost:F4} before basis changes; no upper limit.";
        if (reducedCost < 0)
            return $"Variable x{varIndex} (nonbasic): cost coefficient can increase to {originalCj - reducedCost:F4} before basis changes; no lower limit.";
        return $"Variable x{varIndex} (nonbasic): reduced cost is zero, coefficient can vary freely without affecting optimality (degenerate case).";
    }

    // 8. 
    public string NonBasicVariableCostChange(int varIndex, double newCj, double[] Aj)
    {
        double dot = 0;
        for (int i = 0; i < _result.CBV_BInverse.Length; i++)
            dot += _result.CBV_BInverse[i] * Aj[i];

        double old = _result.CjStar[varIndex];
        _result.CjStar[varIndex] = newCj - dot;

        return $"Changed cost coefficient of nonbasic variable x{varIndex} to {newCj:F4}. " +
               $"Reduced cost updated from {old:F4} → {_result.CjStar[varIndex]:F4}.";
    }
}
