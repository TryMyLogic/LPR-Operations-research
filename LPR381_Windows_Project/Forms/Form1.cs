using LPR381_Windows_Project.Algorithms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPR381_Windows_Project
{
    public partial class Form1 : Form
    {
        private LinearModel model;
        private string outputText = string.Empty;
        private Dictionary<string, ISolver> solvers = new Dictionary<string, ISolver>();

        public Form1()
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
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    txtFilePath.Text = path;

                    try
                    {
                        var parser = new FileParser();
                        model = parser.Parse(path);

                        rtbOutput.Text = $"Model Type: {model.Type}\nObjective: {string.Join(", ", model.ObjectiveCoefficients)}\nConstraints: {model.Constraints.Count}\nSigns: {string.Join(" ", model.SignRestrictions)}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Open Text File Error: " + ex.Message);
                    }
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
                MessageBox.Show("No output to save. Solve the model first.", "Warning");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, outputText);
                        txtSaveLocation.Text = saveFileDialog.FileName;
                        MessageBox.Show("Output saved successfully.", "Success");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving file: " + ex.Message, "Error");
                    }
                }
            }
        }

        private void SolveAndDisplay(string algorithm)
        {
            if (model == null)
            {
                MessageBox.Show("Please load an input file first.", "Warning");
                return;
            }

            if (solvers.TryGetValue(algorithm, out ISolver solver))
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
}
