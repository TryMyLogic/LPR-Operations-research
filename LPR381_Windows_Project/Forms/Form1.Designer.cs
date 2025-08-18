namespace LPR381_Windows_Project
{
    partial class Form1
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
            this.btnFileUpload = new System.Windows.Forms.Button();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSaveLocation = new System.Windows.Forms.Button();
            this.btnSimplex = new System.Windows.Forms.Button();
            this.btnRevisedSimplex = new System.Windows.Forms.Button();
            this.btnB_B = new System.Windows.Forms.Button();
            this.btnCuttingPlane = new System.Windows.Forms.Button();
            this.btnB_B_B = new System.Windows.Forms.Button();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.lblOutputHeading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFileUpload
            // 
            this.btnFileUpload.Location = new System.Drawing.Point(71, 62);
            this.btnFileUpload.Name = "btnFileUpload";
            this.btnFileUpload.Size = new System.Drawing.Size(166, 47);
            this.btnFileUpload.TabIndex = 0;
            this.btnFileUpload.Text = "Upload Textfile";
            this.btnFileUpload.UseVisualStyleBackColor = true;
            this.btnFileUpload.Click += new System.EventHandler(this.btnFileUpload_Click);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Location = new System.Drawing.Point(872, 52);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(767, 668);
            this.rtbOutput.TabIndex = 1;
            this.rtbOutput.Text = "";
            this.rtbOutput.TextChanged += new System.EventHandler(this.rtbOutput_TextChanged);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(252, 76);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(280, 20);
            this.txtFilePath.TabIndex = 2;
            // 
            // btnSaveLocation
            // 
            this.btnSaveLocation.Location = new System.Drawing.Point(71, 115);
            this.btnSaveLocation.Name = "btnSaveLocation";
            this.btnSaveLocation.Size = new System.Drawing.Size(166, 47);
            this.btnSaveLocation.TabIndex = 3;
            this.btnSaveLocation.Text = "Save Location";
            this.btnSaveLocation.UseVisualStyleBackColor = true;
            this.btnSaveLocation.Click += new System.EventHandler(this.btnSaveLocation_Click);
            // 
            // btnSimplex
            // 
            this.btnSimplex.Location = new System.Drawing.Point(162, 497);
            this.btnSimplex.Name = "btnSimplex";
            this.btnSimplex.Size = new System.Drawing.Size(221, 83);
            this.btnSimplex.TabIndex = 4;
            this.btnSimplex.Text = "Simplex Algorithm";
            this.btnSimplex.UseVisualStyleBackColor = true;
            this.btnSimplex.Click += new System.EventHandler(this.btnSimplex_Click);
            // 
            // btnRevisedSimplex
            // 
            this.btnRevisedSimplex.Location = new System.Drawing.Point(162, 586);
            this.btnRevisedSimplex.Name = "btnRevisedSimplex";
            this.btnRevisedSimplex.Size = new System.Drawing.Size(221, 83);
            this.btnRevisedSimplex.TabIndex = 5;
            this.btnRevisedSimplex.Text = "Revised Simplex Algorithm";
            this.btnRevisedSimplex.UseVisualStyleBackColor = true;
            this.btnRevisedSimplex.Click += new System.EventHandler(this.btnRevisedSimplex_Click);
            // 
            // btnB_B
            // 
            this.btnB_B.Location = new System.Drawing.Point(389, 497);
            this.btnB_B.Name = "btnB_B";
            this.btnB_B.Size = new System.Drawing.Size(221, 83);
            this.btnB_B.TabIndex = 6;
            this.btnB_B.Text = "Branch And Bound Simplex Algorithm";
            this.btnB_B.UseVisualStyleBackColor = true;
            this.btnB_B.Click += new System.EventHandler(this.btnB_B_Click);
            // 
            // btnCuttingPlane
            // 
            this.btnCuttingPlane.Location = new System.Drawing.Point(616, 497);
            this.btnCuttingPlane.Name = "btnCuttingPlane";
            this.btnCuttingPlane.Size = new System.Drawing.Size(221, 83);
            this.btnCuttingPlane.TabIndex = 7;
            this.btnCuttingPlane.Text = "Cutting Plane Algorithm";
            this.btnCuttingPlane.UseVisualStyleBackColor = true;
            this.btnCuttingPlane.Click += new System.EventHandler(this.btnCuttingPlane_Click);
            // 
            // btnB_B_B
            // 
            this.btnB_B_B.Location = new System.Drawing.Point(389, 586);
            this.btnB_B_B.Name = "btnB_B_B";
            this.btnB_B_B.Size = new System.Drawing.Size(221, 83);
            this.btnB_B_B.TabIndex = 8;
            this.btnB_B_B.Text = "Knapsack Branch and Bound Algorithm";
            this.btnB_B_B.UseVisualStyleBackColor = true;
            this.btnB_B_B.Click += new System.EventHandler(this.btnB_B_B_Click);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Location = new System.Drawing.Point(252, 129);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(280, 20);
            this.txtSaveLocation.TabIndex = 10;
            // 
            // lblOutputHeading
            // 
            this.lblOutputHeading.AutoSize = true;
            this.lblOutputHeading.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputHeading.Location = new System.Drawing.Point(867, 20);
            this.lblOutputHeading.Name = "lblOutputHeading";
            this.lblOutputHeading.Size = new System.Drawing.Size(537, 29);
            this.lblOutputHeading.TabIndex = 11;
            this.lblOutputHeading.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1838, 770);
            this.Controls.Add(this.lblOutputHeading);
            this.Controls.Add(this.txtSaveLocation);
            this.Controls.Add(this.btnB_B_B);
            this.Controls.Add(this.btnCuttingPlane);
            this.Controls.Add(this.btnB_B);
            this.Controls.Add(this.btnRevisedSimplex);
            this.Controls.Add(this.btnSimplex);
            this.Controls.Add(this.btnSaveLocation);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.btnFileUpload);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
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