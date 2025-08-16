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

//double[,] xValues =
//{
//    { 0, 0 },         // Z row for x1, x2
//    { 1, 0 },         // Row 1
//    { 0, 1 },         // Row 2
//    { 0, 0 }          // Row 3
//};

//double[,] eValues =
//{
//    { -5, -7.5 },     // Z row for e1, e2
//    { -0.15, 0.025 }, // Row 1
//    { 0.025, -0.0875 },// Row 2
//    { 0.15, -0.025 }  // Row 3
//};

//double[,] sValues =
//{
//    { 0 },            // Z row for S1
//    { 0 },            // Row 1
//    { 0 },            // Row 2
//    { 1 }             // Row 3
//};

//double[] rhsValues =
//{
//    320,   // Z row RHS
//    3.6,   // Row 1 RHS
//    1.4,   // Row 2 RHS
//    -0.6   // Row 3 RHS
//};

DualSimplex.Solve(xValues, rhsValues, eValues: eValues);