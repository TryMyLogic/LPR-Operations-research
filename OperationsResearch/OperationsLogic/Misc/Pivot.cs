namespace OperationsLogic.Misc;

public class Pivot
{
    public static void Perform(double[,] tableau, int pivotRow, int pivotCol)
    {
        int rows = tableau.GetLength(0);
        int cols = tableau.GetLength(1);
        double pivotValue = tableau[pivotRow, pivotCol];

        for (int j = 0; j < cols; j++)
            tableau[pivotRow, j] /= pivotValue;

        for (int i = 0; i < rows; i++)
        {
            if (i == pivotRow) continue;
            double factor = tableau[i, pivotCol];
            for (int j = 0; j < cols; j++)
                tableau[i, j] -= factor * tableau[pivotRow, j];
        }
    }
}