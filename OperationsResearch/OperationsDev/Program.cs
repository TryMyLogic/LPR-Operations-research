// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.
// Keep clear before requesting a PR to master.

using OperationsLogic.Algorithms;
using OperationsLogic.Model;

string[] lines =
[
"min -50 -100",
"+7 +2 >= 28",
"+2 +12 >= 24",
"+ +"
];

CanonicalTableau tableau = new(lines);

(string output, tableau) = DualSimplex.Solve(tableau);

double[] coeff = [1, 0];
string relation = "<=";
double rhs = 3;

Console.WriteLine(output);

CanonicalTableau result = tableau.AddConstraint(coeff,relation,rhs);
result.DisplayTableau(printToConsole:true);
