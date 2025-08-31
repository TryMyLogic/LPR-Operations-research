using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public class SolverResult
{
    public string AlgorithmUsed { get; set; }
    public List<string> Steps { get; set; } = new();
    public double FinalValue { get; set; }
    public string GetFormattedOutput()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Algorithm Used: {AlgorithmUsed}");
        sb.AppendLine(new string('-', 50));

        for (int i = 0; i < Steps.Count; i++)
        {
            sb.AppendLine(Steps[i]);
        }

        sb.AppendLine(new string('-', 50));
        sb.AppendLine($"Final Result: {FinalValue:F6}");
        return sb.ToString();
    }
}
