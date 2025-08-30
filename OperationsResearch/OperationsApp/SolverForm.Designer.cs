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
            button3 = new Button();
            btnExit = new Button();
            tabBaB = new TabPage();
            label1 = new Label();
            rtbBranchAndBound = new RichTextBox();
            textBox1 = new TextBox();
            button6 = new Button();
            button4 = new Button();
            button1 = new Button();
            tabCuttingPlane = new TabPage();
            label2 = new Label();
            richTextBox2 = new RichTextBox();
            textBox2 = new TextBox();
            button7 = new Button();
            button5 = new Button();
            button2 = new Button();
            tabSenAnalysis = new TabPage();
            richTextBox1 = new RichTextBox();
            label3 = new Label();
            btnSensitivity = new Button();
            tabControl1.SuspendLayout();
            tabSimplex.SuspendLayout();
            tabBaB.SuspendLayout();
            tabCuttingPlane.SuspendLayout();
            tabSenAnalysis.SuspendLayout();
            SuspendLayout();
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(326, 79);
            rtbOutput.Margin = new Padding(5, 4, 5, 4);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(667, 941);
            rtbOutput.TabIndex = 1;
            rtbOutput.Text = "";
            rtbOutput.TextChanged += rtbOutput_TextChanged;
            // 
            // btnSaveLocation
            // 
            btnSaveLocation.Location = new Point(326, 1039);
            btnSaveLocation.Margin = new Padding(5, 4, 5, 4);
            btnSaveLocation.Name = "btnSaveLocation";
            btnSaveLocation.Size = new Size(222, 72);
            btnSaveLocation.TabIndex = 3;
            btnSaveLocation.Text = "Save Location";
            btnSaveLocation.UseVisualStyleBackColor = true;
            btnSaveLocation.Click += btnSaveLocation_Click;
            // 
            // btnSimplex
            // 
            btnSimplex.Location = new Point(22, 757);
            btnSimplex.Margin = new Padding(5, 4, 5, 4);
            btnSimplex.Name = "btnSimplex";
            btnSimplex.Size = new Size(295, 128);
            btnSimplex.TabIndex = 4;
            btnSimplex.Text = "Simplex Algorithm";
            btnSimplex.UseVisualStyleBackColor = true;
            btnSimplex.Click += btnSimplex_Click;
            // 
            // btnRevisedSimplex
            // 
            btnRevisedSimplex.Location = new Point(22, 893);
            btnRevisedSimplex.Margin = new Padding(5, 4, 5, 4);
            btnRevisedSimplex.Name = "btnRevisedSimplex";
            btnRevisedSimplex.Size = new Size(295, 128);
            btnRevisedSimplex.TabIndex = 5;
            btnRevisedSimplex.Text = "Revised Simplex Algorithm";
            btnRevisedSimplex.UseVisualStyleBackColor = true;
            btnRevisedSimplex.Click += btnRevisedSimplex_Click;
            // 
            // btnB_B
            // 
            btnB_B.Location = new Point(21, 759);
            btnB_B.Margin = new Padding(5, 4, 5, 4);
            btnB_B.Name = "btnB_B";
            btnB_B.Size = new Size(295, 128);
            btnB_B.TabIndex = 6;
            btnB_B.Text = "Branch And Bound Simplex Algorithm";
            btnB_B.UseVisualStyleBackColor = true;
            btnB_B.Click += btnB_B_Click;
            // 
            // btnCuttingPlane
            // 
            btnCuttingPlane.Location = new Point(16, 905);
            btnCuttingPlane.Margin = new Padding(5, 4, 5, 4);
            btnCuttingPlane.Name = "btnCuttingPlane";
            btnCuttingPlane.Size = new Size(295, 128);
            btnCuttingPlane.TabIndex = 7;
            btnCuttingPlane.Text = "Cutting Plane Algorithm";
            btnCuttingPlane.UseVisualStyleBackColor = true;
            btnCuttingPlane.Click += btnCuttingPlane_Click;
            // 
            // btnB_B_B
            // 
            btnB_B_B.Location = new Point(21, 895);
            btnB_B_B.Margin = new Padding(5, 4, 5, 4);
            btnB_B_B.Name = "btnB_B_B";
            btnB_B_B.Size = new Size(295, 128);
            btnB_B_B.TabIndex = 8;
            btnB_B_B.Text = "Knapsack Branch and Bound Algorithm";
            btnB_B_B.UseVisualStyleBackColor = true;
            btnB_B_B.Click += btnB_B_B_Click;
            // 
            // txtSaveLocation
            // 
            txtSaveLocation.Location = new Point(557, 1072);
            txtSaveLocation.Margin = new Padding(5, 4, 5, 4);
            txtSaveLocation.Name = "txtSaveLocation";
            txtSaveLocation.Size = new Size(436, 34);
            txtSaveLocation.TabIndex = 10;
            // 
            // lblOutputHeading
            // 
            lblOutputHeading.AutoSize = true;
            lblOutputHeading.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblOutputHeading.Location = new Point(326, 36);
            lblOutputHeading.Margin = new Padding(5, 0, 5, 0);
            lblOutputHeading.Name = "lblOutputHeading";
            lblOutputHeading.Size = new Size(665, 36);
            lblOutputHeading.TabIndex = 11;
            lblOutputHeading.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSimplex);
            tabControl1.Controls.Add(tabBaB);
            tabControl1.Controls.Add(tabCuttingPlane);
            tabControl1.Controls.Add(tabSenAnalysis);
            tabControl1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(1, 0);
            tabControl1.Margin = new Padding(3, 4, 3, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1975, 1183);
            tabControl1.TabIndex = 12;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabSimplex
            // 
            tabSimplex.BackColor = SystemColors.ActiveCaption;
            tabSimplex.Controls.Add(button3);
            tabSimplex.Controls.Add(btnExit);
            tabSimplex.Controls.Add(btnSimplex);
            tabSimplex.Controls.Add(lblOutputHeading);
            tabSimplex.Controls.Add(btnRevisedSimplex);
            tabSimplex.Controls.Add(rtbOutput);
            tabSimplex.Controls.Add(txtSaveLocation);
            tabSimplex.Controls.Add(btnSaveLocation);
            tabSimplex.Location = new Point(4, 37);
            tabSimplex.Margin = new Padding(3, 4, 3, 4);
            tabSimplex.Name = "tabSimplex";
            tabSimplex.Padding = new Padding(3, 4, 3, 4);
            tabSimplex.Size = new Size(1967, 1142);
            tabSimplex.TabIndex = 0;
            tabSimplex.Text = " Simplex Section";
            // 
            // button3
            // 
            button3.Location = new Point(1647, 1069);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(154, 64);
            button3.TabIndex = 14;
            button3.Text = "Back";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(1808, 1069);
            btnExit.Margin = new Padding(3, 4, 3, 4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(154, 64);
            btnExit.TabIndex = 13;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // tabBaB
            // 
            tabBaB.BackColor = SystemColors.ActiveCaption;
            tabBaB.Controls.Add(label1);
            tabBaB.Controls.Add(rtbBranchAndBound);
            tabBaB.Controls.Add(textBox1);
            tabBaB.Controls.Add(button6);
            tabBaB.Controls.Add(button4);
            tabBaB.Controls.Add(button1);
            tabBaB.Controls.Add(btnB_B);
            tabBaB.Controls.Add(btnB_B_B);
            tabBaB.Location = new Point(4, 37);
            tabBaB.Margin = new Padding(3, 4, 3, 4);
            tabBaB.Name = "tabBaB";
            tabBaB.Padding = new Padding(3, 4, 3, 4);
            tabBaB.Size = new Size(1967, 1142);
            tabBaB.TabIndex = 1;
            tabBaB.Text = "Branch And Bound Section";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(325, 37);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(665, 36);
            label1.TabIndex = 19;
            label1.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // rtbBranchAndBound
            // 
            rtbBranchAndBound.Location = new Point(325, 80);
            rtbBranchAndBound.Margin = new Padding(5, 4, 5, 4);
            rtbBranchAndBound.Name = "rtbBranchAndBound";
            rtbBranchAndBound.Size = new Size(667, 941);
            rtbBranchAndBound.TabIndex = 16;
            rtbBranchAndBound.Text = "";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(555, 1073);
            textBox1.Margin = new Padding(5, 4, 5, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(436, 34);
            textBox1.TabIndex = 18;
            // 
            // button6
            // 
            button6.Location = new Point(325, 1040);
            button6.Margin = new Padding(5, 4, 5, 4);
            button6.Name = "button6";
            button6.Size = new Size(222, 72);
            button6.TabIndex = 17;
            button6.Text = "Save Location";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1647, 1069);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new Size(154, 64);
            button4.TabIndex = 15;
            button4.Text = "Back";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(1808, 1069);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(154, 64);
            button1.TabIndex = 14;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabCuttingPlane
            // 
            tabCuttingPlane.BackColor = SystemColors.ActiveCaption;
            tabCuttingPlane.Controls.Add(label2);
            tabCuttingPlane.Controls.Add(richTextBox2);
            tabCuttingPlane.Controls.Add(textBox2);
            tabCuttingPlane.Controls.Add(button7);
            tabCuttingPlane.Controls.Add(button5);
            tabCuttingPlane.Controls.Add(button2);
            tabCuttingPlane.Controls.Add(btnCuttingPlane);
            tabCuttingPlane.Location = new Point(4, 37);
            tabCuttingPlane.Margin = new Padding(3, 4, 3, 4);
            tabCuttingPlane.Name = "tabCuttingPlane";
            tabCuttingPlane.Size = new Size(1967, 1142);
            tabCuttingPlane.TabIndex = 2;
            tabCuttingPlane.Text = "Cutting Plane Section";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label2.Location = new Point(320, 48);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(665, 36);
            label2.TabIndex = 19;
            label2.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // richTextBox2
            // 
            richTextBox2.Location = new Point(320, 91);
            richTextBox2.Margin = new Padding(5, 4, 5, 4);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(667, 941);
            richTextBox2.TabIndex = 16;
            richTextBox2.Text = "";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(551, 1084);
            textBox2.Margin = new Padding(5, 4, 5, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(436, 34);
            textBox2.TabIndex = 18;
            // 
            // button7
            // 
            button7.Location = new Point(320, 1051);
            button7.Margin = new Padding(5, 4, 5, 4);
            button7.Name = "button7";
            button7.Size = new Size(222, 72);
            button7.TabIndex = 17;
            button7.Text = "Save Location";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1647, 1069);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new Size(154, 64);
            button5.TabIndex = 15;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(1808, 1069);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(154, 64);
            button2.TabIndex = 14;
            button2.Text = "Exit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabSenAnalysis
            // 
            tabSenAnalysis.BackColor = SystemColors.ActiveCaption;
            tabSenAnalysis.Controls.Add(btnSensitivity);
            tabSenAnalysis.Controls.Add(label3);
            tabSenAnalysis.Controls.Add(richTextBox1);
            tabSenAnalysis.Location = new Point(4, 37);
            tabSenAnalysis.Margin = new Padding(3, 4, 3, 4);
            tabSenAnalysis.Name = "tabSenAnalysis";
            tabSenAnalysis.Size = new Size(1967, 1142);
            tabSenAnalysis.TabIndex = 3;
            tabSenAnalysis.Text = "Sensitivity Analysis";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(309, 73);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(610, 883);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline);
            label3.Location = new Point(481, 34);
            label3.Name = "label3";
            label3.Size = new Size(271, 36);
            label3.TabIndex = 1;
            label3.Text = "Sensitivity Analysis";
            // 
            // btnSensitivity
            // 
            btnSensitivity.Location = new Point(497, 962);
            btnSensitivity.Name = "btnSensitivity";
            btnSensitivity.Size = new Size(238, 128);
            btnSensitivity.TabIndex = 2;
            btnSensitivity.Text = "Do Sensitivity Analysis";
            btnSensitivity.UseVisualStyleBackColor = true;
            btnSensitivity.Click += btnSensitivity_Click;
            // 
            // SolverForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1977, 1184);
            Controls.Add(tabControl1);
            Margin = new Padding(5, 4, 5, 4);
            Name = "SolverForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SolverForm";
            WindowState = FormWindowState.Maximized;
            tabControl1.ResumeLayout(false);
            tabSimplex.ResumeLayout(false);
            tabSimplex.PerformLayout();
            tabBaB.ResumeLayout(false);
            tabBaB.PerformLayout();
            tabCuttingPlane.ResumeLayout(false);
            tabCuttingPlane.PerformLayout();
            tabSenAnalysis.ResumeLayout(false);
            tabSenAnalysis.PerformLayout();
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
        private Button btnExit;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Label label1;
        private RichTextBox rtbBranchAndBound;
        private TextBox textBox1;
        private Button button6;
        private Label label2;
        private RichTextBox richTextBox2;
        private TextBox textBox2;
        private Button button7;
        private TabPage tabSenAnalysis;
        private Label label3;
        private RichTextBox richTextBox1;
        private Button btnSensitivity;
    }
}