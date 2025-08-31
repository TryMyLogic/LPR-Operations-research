using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationsLogic.Bonus_NLP;
public class NonLinearProblem
{
    public Func<double[], double> Objective { get; }
    public Func<double[], double[]> Gradient { get; }

    public string ObjectiveText { get; }    // Original objective string
    public string[] GradientText { get; }   // Original gradient strings

    public NonLinearProblem(Func<double[], double> objective,
                            Func<double[], double[]> gradient,
                            string objectiveText,
                            string[] gradientText)
    {
        Objective = objective;
        Gradient = gradient;
        ObjectiveText = objectiveText;
        GradientText = gradientText;
    }
}
