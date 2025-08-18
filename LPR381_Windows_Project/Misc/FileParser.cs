using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Windows_Project.Algorithms
{
    public class FileParser
    {
        public LinearModel Parse(string path)
        {
            string[] lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

            if (lines.Length < 2)
                throw new Exception("Invalid file format: Insufficient lines");

            string[] firstLine = lines[0].Split(' ');
            string type = firstLine[0].ToLower();
            List<double> objectiveCoefficients = new List<double>();
            for (int i = 1; i < firstLine.Length; i++)
            {
                string token = firstLine[i];
                char sign = token[0];
                double coeff = double.Parse(token.Substring(1));
                objectiveCoefficients.Add(sign == '+' ? coeff : -coeff);
            }

            List<Misc.Constraint> constraints = new List<Misc.Constraint>();
            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] parts = lines[i].Split(' ');
                if (parts.Length < objectiveCoefficients.Count + 2)
                    throw new Exception($"Invalid constraint format at line {i + 1}");

                List<double> coeffs = new List<double>();
                for (int j = 0; j < objectiveCoefficients.Count; j++)
                {
                    string token = parts[j];
                    char sign = token[0];
                    double coeff = double.Parse(token.Substring(1));
                    coeffs.Add(sign == '+' ? coeff : -coeff);
                }

                string relation = parts[objectiveCoefficients.Count];
                double rhs = double.Parse(parts[objectiveCoefficients.Count + 1]);
                constraints.Add(new Misc.Constraint(coeffs, relation, rhs));
            }

            string[] signRestrictions = lines[lines.Length - 1].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

            return new LinearModel(type, objectiveCoefficients, constraints, signRestrictions);
        }
    }
}
