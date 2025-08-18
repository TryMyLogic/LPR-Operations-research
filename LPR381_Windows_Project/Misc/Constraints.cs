using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Windows_Project.Misc
{
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
}
