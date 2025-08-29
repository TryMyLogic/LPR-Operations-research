using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LPR381_Windows_Project;

using OperationsLogic.Algorithms;
using OperationsLogic.Misc;

namespace OperationsApp;
public partial class startingForm : Form
{
    public LinearModel model1;
    private string outputText = string.Empty;
    private readonly Dictionary<string, ISolver> solvers = [];
    public startingForm()
    {
        InitializeComponent();
        btnContinue.Enabled = false;
        this.StartPosition = FormStartPosition.CenterScreen;
    }

    private void btnUploadFile_Click(object sender, EventArgs e)
    {
        using OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string path = openFileDialog.FileName;
            txtFileOutput.Text = path;

            try
            {
                FileParser parser = new();
                model1 = parser.Parse(path);

                rtbOutput.Text = $"Model Type: {model1.Type}\nObjective: {string.Join(", ", model1.ObjectiveCoefficients)}\nConstraints: {model1.Constraints.Count}\nSigns: {string.Join(" ", model1.SignRestrictions)}";
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Open Text File Error: " + ex.Message);
            }
        }

        if (txtFileOutput.Text != string.Empty)
        {
            btnContinue.Enabled = true;
        }
    }

    private void btnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
        SolverForm solverForm = new SolverForm(model1);
        solverForm.Show();
        this.Hide();

    }
}
