using OperationsApp;

using OperationsLogic;
using OperationsLogic.Algorithms;
using OperationsLogic.Misc;
using OperationsLogic.Model;

namespace LPR381_Windows_Project;

public partial class SolverForm : Form
{
    private LinearModel model;
    private string outputText = string.Empty;
    private readonly Dictionary<string, ISolver> solvers = [];

    public SolverForm(LinearModel model1)
    {
        InitializeComponent();
        InitializeSolvers();
        this.StartPosition = FormStartPosition.CenterScreen;
        this.WindowState = FormWindowState.Normal;

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

        SolveAndDisplayBandB("Branch And Bound Simplex Algorithm");
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
    private void SolveAndDisplayBandB(string algorithm)
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
                Branch_Bound bab = new Branch_Bound();
                CanonicalTableau CanonicalTableau = new CanonicalTableau();
                CanonicalTableau.DecisionVars = ((SimplexSolver)solver).NumDecisionVars;
                CanonicalTableau.SlackVars = ((SimplexSolver)solver).NumSlackVars;
                CanonicalTableau.ExcessVars = ((SimplexSolver)solver).NumExcessVars;
                CanonicalTableau.IsMaximization = ((SimplexSolver)solver).IsMax;
                var table = ((SimplexSolver)solver).FinalTableau;
                for (int i = 0; i < table.GetLength(0); i++)
                {
                    for (int j = 0; j < table.GetLength(1); j++)
                    {
                        table[i, j] = CanonicalTableau.Tableau[i, j];
                    }
                }

                bab.B_BProcess(CanonicalTableau);
                bab.CompletedBranches.ForEach(branch =>
                {
                    rtbOutput.Text += $"\nCompleted Branch - {branch.Status}\n";
                    rtbOutput.Text += branch.Tableau.DisplayTableau() + "\n";
                });
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
}
