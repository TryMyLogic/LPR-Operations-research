using OperationsApp;

using OperationsLogic.Algorithms;
using OperationsLogic.Bonus_NLP;
using OperationsLogic.Misc;

namespace LPR381_Windows_Project;

public partial class SolverForm : Form
{
    private LinearModel model;
    private string outputText = string.Empty;
    private readonly Dictionary<string, ISolver> solvers = [];

    private NonLinearProblem _nlpProblem;
    private NonLinearSolverManager _nlpSolverManager;
    private string _outputText = string.Empty;

    public SolverForm(LinearModel model1)
    {
        InitializeComponent();
        InitializeSolvers();
        this.StartPosition = FormStartPosition.CenterScreen;
        this.WindowState = FormWindowState.Normal;

        _nlpSolverManager = new NonLinearSolverManager();

        model = model1;
    }

    private void InitializeSolvers()
    {
        solvers.Add("Simplex Algorithm", new SimplexSolver());
        solvers.Add("Revised Simplex Algorithm", new RevisedSimplexSolver());
        solvers.Add("Branch And Bound Simplex Algorithm", new SimplexSolver());
        solvers.Add("Cutting Plane Algorithm", new SimplexSolver());
        solvers.Add("Knapsack Branch and Bound Algorithm", new KnapsackSolver());
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
        SolveAndDisplay("Knapsack Branch and Bound Algorithm");
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

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void SolverForm_Load(object sender, EventArgs e)
    {

    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        this.Hide();
        startingForm startingForm = new startingForm();
        startingForm.Show();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        this.Hide();
        startingForm startingForm = new startingForm();
        startingForm.Show();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        this.Hide();
        startingForm startingForm = new startingForm();
        startingForm.Show();
    }

    private void button6_Click(object sender, EventArgs e)
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

    private void button7_Click(object sender, EventArgs e)
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

    private void btnLoadTextfileNLP_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new()
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                NLPFileParser parser = new();
                _nlpProblem = parser.Parse(openFileDialog.FileName);

                rtbPreviewNLP.Text =
                    $"Function loaded successfully:\r\n" +
                    $"Objective: {_nlpProblem.ObjectiveText}\r\n" +
                    $"Gradients:\r\n{string.Join(Environment.NewLine, _nlpProblem.GradientText)}";

                txtDisplayFileLocationNLP.Text = openFileDialog.FileName;

                _nlpSolverManager.SetProblem(_nlpProblem);

                btnSolveNLP.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Open Text File Error: " + ex.Message);
            }
        }
    }

    private void txtDisplayFileLocationNLP_TextChanged(object sender, EventArgs e)
    {

    }

    private void rtbPreviewNLP_TextChanged(object sender, EventArgs e)
    {

    }

    private void btnSaveNLPsolution_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_outputText))
        {
            MessageBox.Show("No solution to save. Solve the function first.");
            return;
        }

        using SaveFileDialog saveFileDialog = new()
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, _outputText);
                txtSaveLocation.Text = saveFileDialog.FileName;
                MessageBox.Show("Output saved successfully.", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message, "Error");
            }
        }
    }

    private void btnSolveNLP_Click(object sender, EventArgs e)
    {
        if (_nlpProblem == null)
        {
            MessageBox.Show("Please load a function file first.");
            return;
        }

        try
        {
            int numVars = CountVariables(_nlpProblem.ObjectiveText);
            double[] initialGuess = new double[numVars];

            // Assign non-zero initial guess for multi-D problems
            if (numVars > 1)
            {
                for (int i = 0; i < numVars; i++)
                    initialGuess[i] = 1.0; // default non-zero
            }
            // For 1D problems, leave initial guess as 0 (Golden Section Search)

            SolverResult result = _nlpSolverManager.Solve(initialGuess);

            _outputText =
                $"Algorithm used: {result.AlgorithmUsed}\r\n" +
                $"Final Value: {result.FinalValue:F6}\r\n" +
                $"Steps:\r\n{string.Join(Environment.NewLine, result.Steps)}";

            rtbPreviewNLP.Text += "\r\n\r\n" + _outputText;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Solve Error: " + ex.Message);
        }
    }

    private int CountVariables(string line)
    {
        int start = line.IndexOf('(');
        int end = line.IndexOf(')');
        if (start < 0 || end < 0 || end <= start)
            throw new Exception("Invalid objective function format.");

        string inside = line[(start + 1)..end].Trim();
        return inside.Split(',', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    private void button9_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void button8_Click(object sender, EventArgs e)
    {
        this.Hide();
        startingForm startingForm = new startingForm();
        startingForm.Show();
    }
}
