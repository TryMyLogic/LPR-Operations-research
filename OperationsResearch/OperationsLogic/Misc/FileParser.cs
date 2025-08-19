namespace OperationsLogic.Misc;

public class FileParser
{
    public LinearModel Parse(string path)
    {
        string[] lines = File.ReadAllLines(path).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

        if (lines.Length < 2)
            throw new Exception("Invalid file format: Insufficient lines");

        string[] firstLine = lines[0].Split(' ');
        string type = firstLine[0].ToLower();
        List<double> objectiveCoefficients = [];
        for (int i = 1; i < firstLine.Length; i++)
        {
            string token = firstLine[i];
            char sign = token[0];
            double coeff = double.Parse(token[1..]);
            objectiveCoefficients.Add(sign == '+' ? coeff : -coeff);
        }

        List<Constraint> constraints = [];
        for (int i = 1; i < lines.Length - 1; i++)
        {
            string[] parts = lines[i].Split(' ');
            if (parts.Length < objectiveCoefficients.Count + 2)
                throw new Exception($"Invalid constraint format at line {i + 1}");

            List<double> coeffs = [];
            for (int j = 0; j < objectiveCoefficients.Count; j++)
            {
                string token = parts[j];
                char sign = token[0];
                double coeff = double.Parse(token[1..]);
                coeffs.Add(sign == '+' ? coeff : -coeff);
            }

            string relation = parts[objectiveCoefficients.Count];
            double rhs = double.Parse(parts[objectiveCoefficients.Count + 1]);
            constraints.Add(new Constraint(coeffs, relation, rhs));
        }

        string[] signRestrictions = lines[^1].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

        return new LinearModel(type, objectiveCoefficients, constraints, signRestrictions);
    }
}
