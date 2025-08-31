// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.

// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.

using OperationsLogic.Model;

//string[] lines =
//[
//"min -50 -100 -20",
//"+7 +2 +3 >= 28",
//"+2 +12 +2 >= 24",
//"+ + +"
//];

//string[] lines =
//[
//"min -50 -100",
//"+7 +2 >= 28",
//"+2 +12 >= 24",
//"+ +"
//];

string[] lines =
    [
    "max +3 +2",
    "+2 +1 <= 100",
    "+1 +1 <= 80",
    "+1 +0 <= 40",
     "+ +"
    ];

CanonicalTableau tableau = new(lines);

double[,] solvedTableau = new double[4, 6]
{
            { 0, 0, 1, 1, 0, 180 },
            { 0, 1, -1, 2, 0, 60 },
            { 0, 0, -1, 1, 1, 20 },
            { 1, 0, 1, -1, 0, 20 }
};

int decisionVars = 2;
int excessVars = 0;
int slackVars = 3;
bool isMaximization = true;
string[] signRestrictions = ["+", "+"];

tableau = new(decisionVars, excessVars, slackVars, isMaximization, signRestrictions, solvedTableau, tableau.NonCanonicalTableau);
Console.WriteLine("Non Canonical");
Console.WriteLine(tableau.DisplayTableau(null, true));

double[] newValues = [1, 0, 0, 1];
CanonicalTableau newTableau = tableau.AddActivity(newValues);
Console.WriteLine("Canonical (After new activity)");
Console.WriteLine(newTableau.DisplayTableau());
