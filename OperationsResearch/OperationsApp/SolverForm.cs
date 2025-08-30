
﻿using OperationsApp;

using OperationsLogic.Algorithms;
using OperationsLogic.Misc;

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
        rtbOutput.Text = "Branch And Bound Simplex Algorithm not implemented yet.";
        var bnb = new Branch_Bound();
        // i neeed the optimal table from simplex/dual simplex to perform the process on it
        //bnb.B_BProcess(initialTableau);

        //foreach (var (status, tableau) in bnb.CompletedBranches)
        //{
            //$"Completed Branch - {status}");
            //tableau.DisplayTableau());
        //}
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
}
