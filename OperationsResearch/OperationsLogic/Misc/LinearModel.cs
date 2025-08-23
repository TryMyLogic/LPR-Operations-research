namespace OperationsLogic.Misc;

public class LinearModel
{
    public string Type { get; }
    public List<double> ObjectiveCoefficients { get; }
    public List<Misc.Constraint> Constraints { get; }
    public string[] SignRestrictions { get; }

    public LinearModel(string type, List<double> objectiveCoefficients, List<Misc.Constraint> constraints, string[] signRestrictions)
    {
        Type = type;
        ObjectiveCoefficients = objectiveCoefficients;
        Constraints = constraints;
        SignRestrictions = signRestrictions;
    }
}
