namespace LPR381_Windows_Project
{
    partial class SolverForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            rtbOutput = new RichTextBox();
            btnSaveLocation = new Button();
            btnSimplex = new Button();
            btnRevisedSimplex = new Button();
            btnB_B = new Button();
            btnCuttingPlane = new Button();
            btnB_B_B = new Button();
            txtSaveLocation = new TextBox();
            lblOutputHeading = new Label();
            tabControl1 = new TabControl();
            tabSimplex = new TabPage();
            tabBaB = new TabPage();
            tabCuttingPlane = new TabPage();
            tabControl1.SuspendLayout();
            tabSimplex.SuspendLayout();
            tabBaB.SuspendLayout();
            tabCuttingPlane.SuspendLayout();
            SuspendLayout();
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(1118, 66);
            rtbOutput.Margin = new Padding(4, 3, 4, 3);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(584, 707);
            rtbOutput.TabIndex = 1;
            rtbOutput.Text = "";
            rtbOutput.TextChanged += rtbOutput_TextChanged;
            // 
            // btnSaveLocation
            // 
            btnSaveLocation.Location = new Point(1118, 779);
            btnSaveLocation.Margin = new Padding(4, 3, 4, 3);
            btnSaveLocation.Name = "btnSaveLocation";
            btnSaveLocation.Size = new Size(194, 54);
            btnSaveLocation.TabIndex = 3;
            btnSaveLocation.Text = "Save Location";
            btnSaveLocation.UseVisualStyleBackColor = true;
            btnSaveLocation.Click += btnSaveLocation_Click;
            // 
            // btnSimplex
            // 
            btnSimplex.Location = new Point(648, 677);
            btnSimplex.Margin = new Padding(4, 3, 4, 3);
            btnSimplex.Name = "btnSimplex";
            btnSimplex.Size = new Size(258, 96);
            btnSimplex.TabIndex = 4;
            btnSimplex.Text = "Simplex Algorithm";
            btnSimplex.UseVisualStyleBackColor = true;
            btnSimplex.Click += btnSimplex_Click;
            // 
            // btnRevisedSimplex
            // 
            btnRevisedSimplex.Location = new Point(382, 677);
            btnRevisedSimplex.Margin = new Padding(4, 3, 4, 3);
            btnRevisedSimplex.Name = "btnRevisedSimplex";
            btnRevisedSimplex.Size = new Size(258, 96);
            btnRevisedSimplex.TabIndex = 5;
            btnRevisedSimplex.Text = "Revised Simplex Algorithm";
            btnRevisedSimplex.UseVisualStyleBackColor = true;
            btnRevisedSimplex.Click += btnRevisedSimplex_Click;
            // 
            // btnB_B
            // 
            btnB_B.Location = new Point(20, 343);
            btnB_B.Margin = new Padding(4, 3, 4, 3);
            btnB_B.Name = "btnB_B";
            btnB_B.Size = new Size(258, 96);
            btnB_B.TabIndex = 6;
            btnB_B.Text = "Branch And Bound Simplex Algorithm";
            btnB_B.UseVisualStyleBackColor = true;
            btnB_B.Click += btnB_B_Click;
            // 
            // btnCuttingPlane
            // 
            btnCuttingPlane.Location = new Point(24, 350);
            btnCuttingPlane.Margin = new Padding(4, 3, 4, 3);
            btnCuttingPlane.Name = "btnCuttingPlane";
            btnCuttingPlane.Size = new Size(258, 96);
            btnCuttingPlane.TabIndex = 7;
            btnCuttingPlane.Text = "Cutting Plane Algorithm";
            btnCuttingPlane.UseVisualStyleBackColor = true;
            btnCuttingPlane.Click += btnCuttingPlane_Click;
            // 
            // btnB_B_B
            // 
            btnB_B_B.Location = new Point(286, 343);
            btnB_B_B.Margin = new Padding(4, 3, 4, 3);
            btnB_B_B.Name = "btnB_B_B";
            btnB_B_B.Size = new Size(258, 96);
            btnB_B_B.TabIndex = 8;
            btnB_B_B.Text = "Knapsack Branch and Bound Algorithm";
            btnB_B_B.UseVisualStyleBackColor = true;
            btnB_B_B.Click += btnB_B_B_Click;
            // 
            // txtSaveLocation
            // 
            txtSaveLocation.Location = new Point(1320, 804);
            txtSaveLocation.Margin = new Padding(4, 3, 4, 3);
            txtSaveLocation.Name = "txtSaveLocation";
            txtSaveLocation.Size = new Size(297, 29);
            txtSaveLocation.TabIndex = 10;
            // 
            // lblOutputHeading
            // 
            lblOutputHeading.AutoSize = true;
            lblOutputHeading.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblOutputHeading.Location = new Point(1118, 34);
            lblOutputHeading.Margin = new Padding(4, 0, 4, 0);
            lblOutputHeading.Name = "lblOutputHeading";
            lblOutputHeading.Size = new Size(537, 29);
            lblOutputHeading.TabIndex = 11;
            lblOutputHeading.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSimplex);
            tabControl1.Controls.Add(tabBaB);
            tabControl1.Controls.Add(tabCuttingPlane);
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(1, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1728, 887);
            tabControl1.TabIndex = 12;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabSimplex
            // 
            tabSimplex.BackColor = SystemColors.ActiveCaption;
            tabSimplex.Controls.Add(btnSimplex);
            tabSimplex.Controls.Add(lblOutputHeading);
            tabSimplex.Controls.Add(btnRevisedSimplex);
            tabSimplex.Controls.Add(rtbOutput);
            tabSimplex.Controls.Add(txtSaveLocation);
            tabSimplex.Controls.Add(btnSaveLocation);
            tabSimplex.Location = new Point(4, 30);
            tabSimplex.Name = "tabSimplex";
            tabSimplex.Padding = new Padding(3);
            tabSimplex.Size = new Size(1720, 853);
            tabSimplex.TabIndex = 0;
            tabSimplex.Text = "Simplex Section";
            // 
            // tabBaB
            // 
            tabBaB.BackColor = SystemColors.ActiveCaption;
            tabBaB.Controls.Add(btnB_B);
            tabBaB.Controls.Add(btnB_B_B);
            tabBaB.Location = new Point(4, 30);
            tabBaB.Name = "tabBaB";
            tabBaB.Padding = new Padding(3);
            tabBaB.Size = new Size(1709, 851);
            tabBaB.TabIndex = 1;
            tabBaB.Text = "Branch And Bound Section";
            // 
            // tabCuttingPlane
            // 
            tabCuttingPlane.BackColor = SystemColors.ActiveCaption;
            tabCuttingPlane.Controls.Add(btnCuttingPlane);
            tabCuttingPlane.Location = new Point(4, 30);
            tabCuttingPlane.Name = "tabCuttingPlane";
            tabCuttingPlane.Size = new Size(1709, 851);
            tabCuttingPlane.TabIndex = 2;
            tabCuttingPlane.Text = "Cutting Plane Section";
            // 
            // SolverForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1730, 888);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "SolverForm";
            Text = "SolverForm";
            WindowState = FormWindowState.Maximized;
            tabControl1.ResumeLayout(false);
            tabSimplex.ResumeLayout(false);
            tabSimplex.PerformLayout();
            tabBaB.ResumeLayout(false);
            tabCuttingPlane.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Button btnSaveLocation;
        private System.Windows.Forms.Button btnSimplex;
        private System.Windows.Forms.Button btnRevisedSimplex;
        private System.Windows.Forms.Button btnB_B;
        private System.Windows.Forms.Button btnCuttingPlane;
        private System.Windows.Forms.Button btnB_B_B;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Label lblOutputHeading;
        private TabControl tabControl1;
        private TabPage tabSimplex;
        private TabPage tabBaB;
        private TabPage tabCuttingPlane;
    }
}