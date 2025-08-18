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
            btnFileUpload = new Button();
            rtbOutput = new RichTextBox();
            txtFilePath = new TextBox();
            btnSaveLocation = new Button();
            btnSimplex = new Button();
            btnRevisedSimplex = new Button();
            btnB_B = new Button();
            btnCuttingPlane = new Button();
            btnB_B_B = new Button();
            txtSaveLocation = new TextBox();
            lblOutputHeading = new Label();
            SuspendLayout();
            // 
            // btnFileUpload
            // 
            btnFileUpload.Location = new Point(83, 72);
            btnFileUpload.Margin = new Padding(4, 3, 4, 3);
            btnFileUpload.Name = "btnFileUpload";
            btnFileUpload.Size = new Size(194, 54);
            btnFileUpload.TabIndex = 0;
            btnFileUpload.Text = "Upload Textfile";
            btnFileUpload.UseVisualStyleBackColor = true;
            btnFileUpload.Click += btnFileUpload_Click;
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(1017, 60);
            rtbOutput.Margin = new Padding(4, 3, 4, 3);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(894, 770);
            rtbOutput.TabIndex = 1;
            rtbOutput.Text = "";
            rtbOutput.TextChanged += rtbOutput_TextChanged;
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(294, 88);
            txtFilePath.Margin = new Padding(4, 3, 4, 3);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(326, 23);
            txtFilePath.TabIndex = 2;
            // 
            // btnSaveLocation
            // 
            btnSaveLocation.Location = new Point(83, 133);
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
            btnSimplex.Location = new Point(189, 573);
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
            btnRevisedSimplex.Location = new Point(189, 676);
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
            btnB_B.Location = new Point(454, 573);
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
            btnCuttingPlane.Location = new Point(719, 573);
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
            btnB_B_B.Location = new Point(454, 676);
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
            txtSaveLocation.Location = new Point(294, 149);
            txtSaveLocation.Margin = new Padding(4, 3, 4, 3);
            txtSaveLocation.Name = "txtSaveLocation";
            txtSaveLocation.Size = new Size(326, 23);
            txtSaveLocation.TabIndex = 10;
            // 
            // lblOutputHeading
            // 
            lblOutputHeading.AutoSize = true;
            lblOutputHeading.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblOutputHeading.Location = new Point(1011, 23);
            lblOutputHeading.Margin = new Padding(4, 0, 4, 0);
            lblOutputHeading.Name = "lblOutputHeading";
            lblOutputHeading.Size = new Size(537, 29);
            lblOutputHeading.TabIndex = 11;
            lblOutputHeading.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // SolverForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1924, 888);
            Controls.Add(lblOutputHeading);
            Controls.Add(txtSaveLocation);
            Controls.Add(btnB_B_B);
            Controls.Add(btnCuttingPlane);
            Controls.Add(btnB_B);
            Controls.Add(btnRevisedSimplex);
            Controls.Add(btnSimplex);
            Controls.Add(btnSaveLocation);
            Controls.Add(txtFilePath);
            Controls.Add(rtbOutput);
            Controls.Add(btnFileUpload);
            Margin = new Padding(4, 3, 4, 3);
            Name = "SolverForm";
            Text = "SolverForm";
            WindowState = FormWindowState.Maximized;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnFileUpload;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSaveLocation;
        private System.Windows.Forms.Button btnSimplex;
        private System.Windows.Forms.Button btnRevisedSimplex;
        private System.Windows.Forms.Button btnB_B;
        private System.Windows.Forms.Button btnCuttingPlane;
        private System.Windows.Forms.Button btnB_B_B;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.Label lblOutputHeading;
    }
}