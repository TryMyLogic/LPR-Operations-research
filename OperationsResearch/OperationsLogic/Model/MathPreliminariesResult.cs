namespace OperationsLogic.Model;
public class MathPreliminariesResult
{
    public required List<int> BasicVariableIndices { get; set; }
    public required double[,] B { get; set; }
    public required double[] CBV { get; set; }
    public required double[,] BInverse { get; set; }
    public required double[] CBV_BInverse { get; set; }
    public required double[,] AjStar { get; set; }
    public required double[] CjStar { get; set; }
    public required double[] BStar { get; set; }
    public required double ZStar { get; set; }

}
