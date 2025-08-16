// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.
// Keep clear before requesting a PR to master.


using OperationsLogic;

double[,] xValues =
{
    { -50, -100 },
    {  -7, -2 },
    {  -2, -12 }
};

double[,] eValues =
{
    { 0, 0 },
    {  1,  0 },
    {  0,  1 }
};

double[] rhsValues =
[
     0,
     -28,
     -24
];

//DualSimplex.PivotOnTable(xValues, rhsValues, eValues: eValues);
DualSimplex.Solve(xValues, rhsValues, eValues: eValues);