using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NCalc;

namespace OperationsLogic.Bonus_NLP;
public class NLPFileParser
{

    public NonLinearProblem Parse(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath)
                             .Where(l => !string.IsNullOrWhiteSpace(l))
                             .ToArray();

        if (lines.Length < 2)
            throw new Exception("File must contain at least Objective and one Gradient line.");

        string objLine = lines[0]; // Original objective text
        Func<double[], double> objective = x => EvaluateExpression(objLine, x);

        string[] gradLines = lines.Skip(1).ToArray();
        Func<double[], double[]> gradient = x =>
        {
            double[] grad = new double[gradLines.Length];
            for (int i = 0; i < gradLines.Length; i++)
            {
                grad[i] = EvaluateExpression(gradLines[i], x);
            }
            return grad;
        };

        return new NonLinearProblem(objective, gradient, objLine, gradLines);
    }

    private double EvaluateExpression(string line, double[] x)
    {
        int eqIndex = line.IndexOf('=');
        if (eqIndex < 0)
            throw new Exception($"Line must contain '=': {line}");

        string expr = line[(eqIndex + 1)..].Trim();

        expr = ConvertCaretToPow(expr);

        Expression e = new(expr);

        for (int i = 0; i < x.Length; i++)
        {
            string varName = x.Length == 1 ? "x" : $"x{i + 1}";
            e.Parameters[varName] = x[i];
        }

        try
        {
            return Convert.ToDouble(e.Evaluate());
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot evaluate expression: {expr}. Error: {ex.Message}");
        }
    }


    private string ConvertCaretToPow(string expr)
    {
        string pattern = @"([\w\.\-]+)\s*\^\s*([\w\.\-]+)";
        return System.Text.RegularExpressions.Regex.Replace(expr, pattern, "Pow($1,$2)");
    }
}
