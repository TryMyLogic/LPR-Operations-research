// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.

// Console app used as an alternative to DisplayApp. It is purely for development purposes and will be dropped in production.

using OperationsLogic.Algorithms;
using OperationsLogic.Misc;
using OperationsLogic.Model;

// Create the tableau matrix
//double[,] tableauData = new double[3, 5]
//{
//    { 0, 0, 1.25, 0.75, 41.25 }, // Z-row: x1, x2, S1, S2, RHS
//    { 0, 1, 2.25, -0.25, 2.25 }, // Row 1: x2 basic
//    { 1, 0, -1.25, 0.25, 3.75 }  // Row 2: x1 basic
//};

//double[,] noncanonicalTableauData = new double[3, 5]
//{
//    { 8, 5, 0,0,0 }, // Z-row: x1, x2, S1, S2, RHS
//    { 1, 1, 1, 0, 6 }, // Row 1: x2 basic
//    {9, 5, 0, 1, 45 }  // Row 2: x1 basic
//};

//double[,] tableauData = new double[3, 5]
//{
//    { -8, -5, 0,0,0 }, // Z-row: x1, x2, S1, S2, RHS
//    { 1, 1, 1, 0, 6 }, // Row 1: x2 basic
//    {9, 5, 0, 1, 45 }  // Row 2: x1 basic
//};

//// Initialize CanonicalTableau
//CanonicalTableau tableau = new(
//    decisionVars: 2,
//    excessVars: 0,
//    slackVars: 2,
//    isMaximization: true,
//    signRestrictions: ["int", "int"],
//    noncanonicaltableau: noncanonicalTableauData,
//    tableau: tableauData
//);
//string output = CuttingPlane.Solve(tableau);
//Console.WriteLine(output);

try
{
    LinearModel model = new(
            type: "max",
            objectiveCoefficients: [8, 5], // Updated for 8x1 + 5x2
            constraints:
            [
                new OperationsLogic.Misc.Constraint([1, 1], "<=", 6),  // x1 + x2 <= 6 (Labour)
            new OperationsLogic.Misc.Constraint([9, 5], "<=", 45), // 9x1 + 5x2 <= 45 (Wood)
            ],
            signRestrictions: ["int", "int"]
        );

    CuttingPlane solver = new();

    solver.Solve(model, out string output);

    Console.WriteLine("Output from CuttingPlane.Solve:");
    Console.WriteLine(output);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

