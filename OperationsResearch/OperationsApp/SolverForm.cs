using OperationsApp;

using OperationsLogic.Algorithms;
using OperationsLogic.Analysis;
using OperationsLogic.Bonus_NLP;
using OperationsLogic.Misc;
using OperationsLogic.Model;

namespace LPR381_Windows_Project;

public partial class SolverForm : Form
{
    private LinearModel model;
    private string outputText = string.Empty;
    private CanonicalTableau? tableauForSensitivity;
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
        solvers.Add("Cutting Plane Algorithm", new CuttingPlane());
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
        SolveAndDisplay("Cutting Plane Algorithm", cuttingPlaneTextbox);
    }

    private void btnB_B_B_Click(object sender, EventArgs e)
    {
        SolveAndDisplay("Knapsack Branch and Bound Algorithm", richTextBox1);
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
    private CanonicalTableau BuildCanonicalTableauFromModel(LinearModel model)
    {
        int n = model.ObjectiveCoefficients.Count;
        int m = model.Constraints.Count;

        int slackCount = model.Constraints.Count(c => c.Relation == "<=");
        int excessCount = model.Constraints.Count(c => c.Relation == ">=");

        int totalVars = n + slackCount + excessCount;
        double[,] tableau = new double[m + 1, totalVars + 1];
        double[,] nonCanonical = new double[m + 1, totalVars + 1];

        bool isMax = model.Type == "max";

        int slackIndex = 0;
        int excessIndex = 0;
        for (int i = 0; i < m; i++)
        {
            var c = model.Constraints[i];
            for (int j = 0; j < n; j++)
            {
                tableau[i, j] = c.Coefficients[j];
                nonCanonical[i, j] = c.Coefficients[j];
            }

            if (c.Relation == "<=")
            {
                tableau[i, n + excessCount + slackIndex] = 1;
                nonCanonical[i, n + excessCount + slackIndex] = 1;
                slackIndex++;
            }
            else if (c.Relation == ">=")
            {
                tableau[i, n + excessIndex] = -1;
                nonCanonical[i, n + excessIndex] = -1;
                excessIndex++;
            }
            else
            {
                throw new InvalidOperationException($"Constraint {i + 1} has unsupported relation {c.Relation}");
            }

            tableau[i, totalVars] = c.RHS;
            nonCanonical[i, totalVars] = c.RHS;
        }

        for (int j = 0; j < n; j++)
            tableau[m, j] = isMax ? -model.ObjectiveCoefficients[j] : model.ObjectiveCoefficients[j];

        tableau[m, totalVars] = 0;

        for (int j = 0; j < n; j++)
            nonCanonical[m, j] = model.ObjectiveCoefficients[j];

        nonCanonical[m, totalVars] = 0;

        return new CanonicalTableau(
            decisionVars: n,
            excessVars: excessCount,
            slackVars: slackCount,
            isMaximization: isMax,
            signRestrictions: model.SignRestrictions,
            tableau: tableau,
            noncanonicaltableau: nonCanonical
        );
    }

    private void SolveAndDisplay(string algorithm, RichTextBox? outputBox = null)
    {
        if (model == null)
        {
            _ = MessageBox.Show("Please load an input file first.", "Warning");
            return;
        }

        RichTextBox targetBox = outputBox ?? rtbOutput;

        if (solvers.TryGetValue(algorithm, out ISolver? solver))
        {
            try
            {
                solver.Solve(model, out outputText);
                targetBox.Text = outputText;
                if (solver is SimplexSolver)
                {
                    tableauForSensitivity = BuildCanonicalTableauFromModel(model);
                }
                else
                {
                    tableauForSensitivity = null;
                }
            }
            catch (Exception ex)
            {
                targetBox.Text = $"Error solving: {ex.Message}";
            }
        }
        else
        {
            targetBox.Text = "Algorithm not implemented.";
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

    private void SolverForm_Load_1(object sender, EventArgs e)
    {

    }

    private void btnSensitivity_Click(object sender, EventArgs e)
    {
        if (tableauForSensitivity == null)
        {
            MessageBox.Show("Solve a model first using Simplex.", "Warning");
            return;
        }

        var basicIndices = tableauForSensitivity.GetBasicVariableIndices();
        var preliminaries = tableauForSensitivity.ComputeMathPreliminaries(basicIndices);
        SensitivityAnalysis sa = new SensitivityAnalysis(model, preliminaries);

        int totalColumns = tableauForSensitivity.Tableau.GetLength(1) - 1;
        int varIndex = (int)nudVarIndex.Value - 1;


        if (varIndex < 0 || varIndex >= totalColumns)
        {
            MessageBox.Show(
                $"Please input a valid variable index between 1 and {totalColumns}",
                "Error");
            return;
        }

        double newValue = (double)nudNewValue.Value;
        if (newValue == 0)
        {
            MessageBox.Show("Please input a valid variable number", "Error");
            return;
        }

        string output;

        try
        {
            if (basicIndices.Contains(varIndex))
            {
                output = sa.ShowRangeBasic(varIndex) + Environment.NewLine +
                         sa.ApplyChangeBasic(varIndex, newValue);
            }
            else
            {
                output = sa.ShowRangeNonBasic(varIndex) + Environment.NewLine +
                         sa.ApplyChangeNonBasic(varIndex, newValue);
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            MessageBox.Show(
                $"Variable x{varIndex + 1} no longer exists in the solved tableau (column removed).",
                "Error");
            return;
        }

        rtbSensitivity.Text = output;
    }
}

