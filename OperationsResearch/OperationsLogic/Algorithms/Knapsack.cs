using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using OperationsLogic.Misc;

namespace OperationsLogic.Algorithms;
public class KnapsackSolver : ISolver
{
    private sealed class KnapsackItem
    {
        public int OriginalIndex { get; }
        public double Weight { get; }
        public double Value { get; }
        public double Ratio => Value / Weight;
        public KnapsackItem(int originalIndex, double weight, double value)
        {
            OriginalIndex = originalIndex;
            Weight = weight;
            Value = value;
        }
        public override string ToString() => $"(idx: {OriginalIndex + 1}, w: {Weight}, v:{Value}, r: {Ratio:F3})";
    }
    public class BBNode
    {
        public int Level;
        public double Weight;
        public double Value;
        public double Bound;
        public List<int> Picks = new();
    }
    private const int Decimals = 3;
    private readonly bool _useBestFirst = true;
    public void Solve(LinearModel model, out string output)
    {
        StringBuilder sb = new StringBuilder();
        if (model.Constraints.Count != 1)
            throw new InvalidOperationException($"Knapsack solver only supports 1 constraint, but {model.Constraints.Count} were provided.");
        Constraint first = model.Constraints[0];
        if (first.Coefficients.Count != model.ObjectiveCoefficients.Count)
            throw new InvalidOperationException("Mismatch between number of objective coefficients (values) and first-constraint coefficients (weights).");
        double capacity = first.RHS;
        List<KnapsackItem> items = new List<KnapsackItem>(model.ObjectiveCoefficients.Count);
        for (int i = 0; i < model.ObjectiveCoefficients.Count; i++)
        {
            double value = model.ObjectiveCoefficients[i];
            double weight = first.Coefficients[i];
            if (weight <= 0)
                throw new InvalidOperationException($"Item {i + 1} has no positive weight ({weight}).");
            items.Add(new KnapsackItem(i, weight, value));
        }
        List<KnapsackItem> sorted = new List<KnapsackItem>(items);
        sorted.Sort((a, b) => b.Ratio.CompareTo(a.Ratio));
        sb.AppendLine("=== Branch and Bound Knapsack ===");
        sb.AppendLine($"Capacity: {Round(capacity)}");
        sb.AppendLine("Items (sorted by value/weight):");
        for (int i = 0; i < sorted.Count; i++)
            sb.AppendLine($" s[{i}] = {sorted[i]}");
        sb.AppendLine();

        PriorityQueue<BBNode, double>? pq = _useBestFirst ? new PriorityQueue<BBNode, double>() : null;
        Queue<BBNode>? q = _useBestFirst ? null : new Queue<BBNode>();
        BBNode root = new BBNode { Level = -1, Weight = 0, Value = 0, Bound = ComputeBound(-1, 0, 0, sorted, capacity) };
        Enqueue(root);
        double bestValue = 0;
        List<int> bestPicks = new List<int>();
        int step = 0;
        while (TryDequeue(out BBNode u))
        {
            if (u.Bound <= bestValue || u.Level == sorted.Count - 1)
                continue;
            
            int next = u.Level + 1;
            BBNode inc = new BBNode
            {
                Level = next,
                Weight = u.Weight + sorted[next].Weight,
                Value = u.Value + sorted[next].Value,
                Picks = new List<int>(u.Picks) { next }
            };
            inc.Bound = ComputeBound(inc.Level, inc.Weight, inc.Value, sorted, capacity);

            step++;
            LogNode(sb, step, sorted[next].OriginalIndex, 1, capacity, sorted, inc.Weight, inc.Value, bound: inc.Bound);
            if (inc.Weight <= capacity && inc.Value > bestValue)
            {
                bestValue = inc.Value;
                bestPicks = [.. inc.Picks];
                sb.AppendLine($" Candidate found: z = {Round(bestValue)}");
                sb.AppendLine();
            }

            bool enqueueInc = inc.Bound > bestValue;
            if (enqueueInc) Enqueue(inc);

            BBNode exc = new BBNode
            {
                Level = next,
                Weight = u.Weight,
                Value = u.Value,
                Picks = [.. u.Picks],
            };
            exc.Bound = ComputeBound(exc.Level, exc.Weight, exc.Value, sorted, capacity);
            step++;
            LogNode(sb, step, sorted[next].OriginalIndex, 0, capacity, sorted, exc.Weight, exc.Value, bound:  exc.Bound);
            bool enqueueExc = exc.Bound > bestValue;
            if(exc.Weight <= capacity && exc.Value > bestValue)
            {
                bestValue = exc.Value;
                bestPicks = [..  exc.Picks];
                sb.AppendLine($" Candidate found: z = {Round(bestValue)}");
                sb.AppendLine();
            }
            if (enqueueExc) Enqueue(exc);
        }
        List<int> chosenOriginalIds = new List<int>();
        double totalW = 0, totalV = 0;
        foreach (int sId in bestPicks)
        {
            KnapsackItem it = sorted[sId];
            chosenOriginalIds.Add(it.OriginalIndex + 1);
            totalW += it.Weight;
            totalV += it.Value;
        }
        chosenOriginalIds.Sort();

        sb.AppendLine();
        sb.AppendLine("=== Best Candidate ===");
        sb.AppendLine($"Best Value : {Round(totalV)}");
        sb.AppendLine($"Best Weight : {Round(totalW)} / {Round(capacity)}");
        sb.AppendLine($"Items Chosen: {string.Join(", ", chosenOriginalIds)}");

        output = sb.ToString();

        void Enqueue(BBNode node)
        {
            if (_useBestFirst)
            {
                pq!.Enqueue(node, -node.Bound);
            }
            else
            {
                q!.Enqueue(node);
            }
        }
        bool TryDequeue(out BBNode node)
        {
            if (_useBestFirst)
            {
                if (pq!.Count > 0)
                {
                    node = pq.Dequeue();
                    return true;
                }
            }
            else
            {
                if (q!.Count > 0)
                {
                    node = q.Dequeue();
                    return true;
                }
            }
            node = null!;
            return false;
        }
    }
    private static double ComputeBound(int level, double weight, double value, List<KnapsackItem> sorted, double capacity)
    {
        if (weight >= capacity) return 0;
        double bound = value;
        double w = weight;
        int j = level + 1;

        while (j < sorted.Count && w + sorted[j].Weight <= capacity)
        {
            w += sorted[j].Weight;
            bound += sorted[j].Value;
            j++;
        }
        if (j < sorted.Count)
        {
            double remain = capacity - w;
            bound += sorted[j].Ratio * remain;
        }
        return bound;
    }
    private static string Round(double x)
    {
        return Math.Round(x, Decimals).ToString($"F{Decimals}");
    }
    private static void LogNode(StringBuilder sb,
        int subProblemId,
        int itemIndex,
        int decision,
        double capacity,
        List<KnapsackItem> sorted,
        double currentWeight,
        double currentValue,
        double bound)
    {
        sb.AppendLine($"Sub-Problem {subProblemId}: x{itemIndex + 1} = {decision}");

        double remaining = capacity - currentWeight;
        if (remaining < 0)
            sb.AppendLine($"  Remaining capacity = {Round(remaining)} (infeasible)");
        else
            sb.AppendLine($"  Remaining capacity = {Round(remaining)}");

            int j = itemIndex + 1;
        double val = currentValue;
        while (j < sorted.Count && remaining >= sorted[j].Weight)
        {
            remaining -= sorted[j].Weight;
            val += sorted[j].Value;
            sb.AppendLine($"  Take x{sorted[j].OriginalIndex + 1} = 1 → cap left = {Round(remaining)}");
            j++;
        }

        if (j < sorted.Count && remaining > 0)
        {
            double fraction = remaining / sorted[j].Weight;
            val += fraction * sorted[j].Value;
            sb.AppendLine($"  Take x{sorted[j].OriginalIndex + 1} = {Round(fraction)} (fractional, for bound only)");
            remaining = 0;
        }

        sb.AppendLine($"  Bound (z) = {Round(val)}");
        sb.AppendLine();
    }
}
