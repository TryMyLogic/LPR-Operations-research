using OperationsLogic.Algorithms;
using OperationsLogic.Misc;

namespace LPR381_Windows_Project;

public partial class SolverForm : Form
{
    private LinearModel model;
    private string outputText = string.Empty;
    private readonly Dictionary<string, ISolver> solvers = [];

    public SolverForm()
    {
        InitializeComponent();
        InitializeSolvers();
    }

    private void InitializeSolvers()
    {
        solvers.Add("Simplex Algorithm", new SimplexSolver());
        solvers.Add("Revised Simplex Algorithm", new RevisedSimplexSolver());
        solvers.Add("Branch And Bound Simplex Algorithm", new SimplexSolver());
        solvers.Add("Cutting Plane Algorithm", new SimplexSolver());
        solvers.Add("Knapsack Branch and Bound Algorithm", new SimplexSolver());
    }

    private void btnFileUpload_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string path = openFileDialog.FileName;
            txtFilePath.Text = path;

            try
            {
                FileParser parser = new();
                model = parser.Parse(path);

                rtbOutput.Text = $"Model Type: {model.Type}\nObjective: {string.Join(", ", model.ObjectiveCoefficients)}\nConstraints: {model.Constraints.Count}\nSigns: {string.Join(" ", model.SignRestrictions)}";
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Open Text File Error: " + ex.Message);
            }
        }
    }

    private void rtbOutput_TextChanged(object sender, EventArgs e)
    {

    }

    private void btnSimplex_Click(object sender, EventArgs e)
    {
        SolveAndDisplay("Simplex Algorithm");
    }

    private void btnRevisedSimplex_Click(object sender, EventArgs e)
    {
        SolveAndDisplay("Revised Simplex Algorithm");
    }

    private void btnB_B_Click(object sender, EventArgs e)
    {
        rtbOutput.Text = "Branch And Bound Simplex Algorithm not implemented yet.";
    }

    private void btnCuttingPlane_Click(object sender, EventArgs e)
    {
        rtbOutput.Text = "Cutting Plane Algorithm not implemented yet.";
    }

    private void btnB_B_B_Click(object sender, EventArgs e)
    {
        rtbOutput.Text = "Knapsack Branch and Bound Algorithm not implemented yet.";
    }

    private void btnSaveLocation_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(outputText))
        {
            _ = MessageBox.Show("No output to save. Solve the model first.", "Warning");
            return;
        }

        using SaveFileDialog saveFileDialog = new();
        saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                File.WriteAllText(saveFileDialog.FileName, outputText);
                txtSaveLocation.Text = saveFileDialog.FileName;
                _ = MessageBox.Show("Output saved successfully.", "Success");
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Error saving file: " + ex.Message, "Error");
            }
        }
    }

    private void SolveAndDisplay(string algorithm)
    {
        if (model == null)
        {
            _ = MessageBox.Show("Please load an input file first.", "Warning");
            return;
        }

        if (solvers.TryGetValue(algorithm, out ISolver? solver))
        {
            try
            {
                solver.Solve(model, out outputText);
                rtbOutput.Text = outputText;
            }
            catch (Exception ex)
            {
                rtbOutput.Text = $"Error solving: {ex.Message}";
            }
        }
        else
        {
            rtbOutput.Text = "Algorithm not implemented.";
        }
    }
}
