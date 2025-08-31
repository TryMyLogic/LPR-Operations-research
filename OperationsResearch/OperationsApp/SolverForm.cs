using OperationsApp;

using OperationsLogic.Algorithms;
using OperationsLogic.Analysis;
using OperationsLogic.Misc;
using OperationsLogic.Model;

namespace LPR381_Windows_Project;

public partial class SolverForm : Form
{
    private LinearModel model;
    private string outputText = string.Empty;
    private CanonicalTableau? tableauForSensitivity;
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
    private CanonicalTableau BuildCanonicalTableauFromModel(LinearModel model)
    {
        int n = model.ObjectiveCoefficients.Count;
        int m = model.Constraints.Count;
        int slackCount = model.Constraints.Count(c => c.Relation == "<=");
        int excessCount = model.Constraints.Count(c => c.Relation == ">=");

        int totalVars = n + slackCount + excessCount;
        double[,] tableau = new double[m + 1, totalVars + 1];
        double[,] nonCanonical = new double[m + 1, totalVars + 1];

        int slackIndex = 0;
        int excessIndex = 0;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                tableau[i + 1, j] = model.Constraints[i].Coefficients[j];
                nonCanonical[i + 1, j] = model.Constraints[i].Coefficients[j];
            }

            string rel = model.Constraints[i].Relation;
            double rhs = model.Constraints[i].RHS;
            if (rel == "<=")
            {
                tableau[i + 1, n + excessCount + slackIndex] = 1;
                nonCanonical[i + 1, n + excessCount + slackIndex] = 1;
                slackIndex++;
            }
            else if (rel == ">=")
            {
                tableau[i + 1, n + excessIndex] = 1;
                nonCanonical[i + 1, n + excessIndex] = -1;
                excessIndex++;
            }

            tableau[i + 1, totalVars] = rhs;
            nonCanonical[i + 1, totalVars] = rhs;
        }

        for (int j = 0; j < n; j++)
        {
            tableau[0, j] = -model.ObjectiveCoefficients[j]; 
            nonCanonical[0, j] = model.ObjectiveCoefficients[j];
        }
        tableau[0, totalVars] = 0;
        nonCanonical[0, totalVars] = 0;

        return new CanonicalTableau(n, excessCount, slackCount, true, model.SignRestrictions, tableau, nonCanonical);
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
                if (algorithm == "Simplex Algorithm" || algorithm == "Revised Simplex Algorithm")
                {
                    tableauForSensitivity = BuildCanonicalTableauFromModel(model);
                }
                switch (algorithm)
                {
                    case "Knapsack Branch and Bound Algorithm":
                        rtbBranchAndBound.Text = outputText;
                        break;
                    default:
                        rtbOutput.Text = outputText;
                        break;
                }
            }
            catch (Exception ex)
            {
                switch (algorithm)
                {
                    case "Knapsack Branch and Bound Algorithm":
                        rtbBranchAndBound.Text = $"Error: {ex.Message}";
                        break;
                    default:
                        rtbOutput.Text = $"Error: {ex.Message}";
                        break;
                }
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

    private void btnSensitivity_Click(object sender, EventArgs e)
    {
        if (tableauForSensitivity == null)
        {
            _ = MessageBox.Show("Solve a model first using Simplex.", "Warning");
            return;
        }

        var preliminaries = tableauForSensitivity.ComputeMathPreliminaries(
            tableauForSensitivity.GetBasicVariableIndices());
        SensitivityAnalysis sa = new SensitivityAnalysis(model, preliminaries);

        int userVarNumber = (int)nudVarIndex.Value; 
        if(userVarNumber == 0)
        {
            MessageBox.Show("Please input a valid variable index", "Error");
            return;
        }
        int varIndex = userVarNumber - 1; 
        

        double newValue = (double)nudNewValue.Value;
        if(newValue == 0)
        {
            MessageBox.Show("Please input a valid variable number", "Error");
            return;
        }

        string output;

        if (preliminaries.BasicVariableIndices.Contains(varIndex))
        {
            output = sa.ShowRangeBasic(varIndex) + Environment.NewLine;
            output += sa.ApplyChangeBasic(varIndex, newValue); 
        }
        else
        {
            output = sa.ShowRangeNonBasic(varIndex) + Environment.NewLine;
            output += sa.ApplyChangeNonBasic(varIndex, newValue); 
        }

        rtbSensitivity.Text = output;
    }
}
