namespace OperationsLogic.Model;
public class MathPreliminariesResult
{
    public required List<int> BasicVariableIndices { get; set; }
    public required double[,] B { get; set; }
    public required double[] CBV { get; set; }
    public required double[,] BInverse { get; set; }
    public required double[] CBV_BInverse { get; set; }
    public required double[,] AjStar { get; set; }
    public required double[] CjStar { get; set; }
    public required double[] BStar { get; set; }
    public required double ZStar { get; set; }

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
        double[] column = new double[BInverse.GetLength(0)];
        for (int i = 0; i < column.Length; i++)
            column[i] = BInverse[i, constraintIndex];

        double min = double.NegativeInfinity;
        double max = double.PositiveInfinity;

        for (int i = 0; i < BStar.Length; i++)
        {
            if (column[i] > 0)
                max = Math.Min(max, BStar[i] / column[i]);
            else if (column[i] < 0)
                min = Math.Max(min, BStar[i] / column[i]);
        }
        return (min, max);
    }

    // 6.
    public string ApplyRHSChange(int constraintIndex, double delta)
    {
        BStar[constraintIndex] += delta;
        // Update Z*
        ZStar = 0;
        for (int i = 0; i < CBV.Length; i++)
            ZStar += CBV[i] * BStar[i];
        return $"Applied change of {delta:+0.####;-0.####;0} to RHS of constraint {constraintIndex}. " +
               $"New RHS = {BStar[constraintIndex]:F4}, new Z* = {ZStar:F4}.";
    }

    // 7. 
    public string NonBasicVariableRange(int varIndex, double originalCj)
    {
        double reducedCost = CjStar[varIndex];
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
        for (int i = 0; i < CBV_BInverse.Length; i++)
            dot += CBV_BInverse[i] * Aj[i];

        double old = CjStar[varIndex];
        CjStar[varIndex] = newCj - dot;

        return $"Changed cost coefficient of nonbasic variable x{varIndex} to {newCj:F4}. " +
               $"Reduced cost updated from {old:F4} → {CjStar[varIndex]:F4}.";
    }
}
