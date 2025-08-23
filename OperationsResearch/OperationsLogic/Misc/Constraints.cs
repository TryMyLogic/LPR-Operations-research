namespace OperationsLogic.Misc;

public class Constraint
{
    public List<double> Coefficients { get; }
    public string Relation { get; }
    public double RHS { get; }

    public Constraint(List<double> coefficients, string relation, double rhs)
    {
        Coefficients = coefficients;
        Relation = relation;
        RHS = rhs;
    }
}
