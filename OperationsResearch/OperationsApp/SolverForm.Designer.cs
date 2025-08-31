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
            tabDuality = new TabPage();
            txtDualtiySaveLocation = new TextBox();
            btnDualitySave = new Button();
            label6 = new Label();
            rtbDualityOutput = new RichTextBox();
            btnDualityBack = new Button();
            btnDualityExit = new Button();
            btnTestDuality = new Button();
            tabBaB = new TabPage();
            label1 = new Label();
            richTextBox1 = new RichTextBox();
            textBox1 = new TextBox();
            button6 = new Button();
            button4 = new Button();
            button1 = new Button();
            tabCuttingPlane = new TabPage();
            label2 = new Label();
            cuttingPlaneTextbox = new RichTextBox();
            textBox2 = new TextBox();
            button7 = new Button();
            button5 = new Button();
            button2 = new Button();
            tabSenAnalysis = new TabPage();
            txtAddResult = new RichTextBox();
            label10 = new Label();
            btnAddActivity = new Button();
            txtActivityValues = new RichTextBox();
            btnAddConstraint = new Button();
            txtRhs = new TextBox();
            txtCoeff = new TextBox();
            cbRelation = new ComboBox();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            btnSensitivity = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            nudNewValue = new NumericUpDown();
            nudVarIndex = new NumericUpDown();
            rtbSensitivity = new RichTextBox();
            tabNLP = new TabPage();
            button8 = new Button();
            button9 = new Button();
            btnSolveNLP = new Button();
            btnSaveNLPsolution = new Button();
            rtbPreviewNLP = new RichTextBox();
            txtDisplayFileLocationNLP = new TextBox();
            btnLoadTextfileNLP = new Button();
            tabControl1.SuspendLayout();
            tabSimplex.SuspendLayout();
            tabDuality.SuspendLayout();
            tabBaB.SuspendLayout();
            tabCuttingPlane.SuspendLayout();
            tabSenAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudNewValue).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudVarIndex).BeginInit();
            tabNLP.SuspendLayout();
            SuspendLayout();
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(285, 59);
            rtbOutput.Margin = new Padding(4, 3, 4, 3);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(584, 707);
            rtbOutput.TabIndex = 1;
            rtbOutput.Text = "";
            rtbOutput.TextChanged += rtbOutput_TextChanged;
            // 
            // btnSaveLocation
            // 
            btnSaveLocation.Location = new Point(285, 779);
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
            btnSimplex.Location = new Point(19, 568);
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
            btnRevisedSimplex.Location = new Point(19, 670);
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
            btnB_B.Location = new Point(18, 569);
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
            btnCuttingPlane.Location = new Point(14, 679);
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
            btnB_B_B.Location = new Point(18, 671);
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
            txtSaveLocation.Location = new Point(487, 804);
            txtSaveLocation.Margin = new Padding(4, 3, 4, 3);
            txtSaveLocation.Name = "txtSaveLocation";
            txtSaveLocation.Size = new Size(382, 29);
            txtSaveLocation.TabIndex = 10;
            // 
            // lblOutputHeading
            // 
            lblOutputHeading.AutoSize = true;
            lblOutputHeading.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            lblOutputHeading.Location = new Point(285, 27);
            lblOutputHeading.Margin = new Padding(4, 0, 4, 0);
            lblOutputHeading.Name = "lblOutputHeading";
            lblOutputHeading.Size = new Size(537, 29);
            lblOutputHeading.TabIndex = 11;
            lblOutputHeading.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabSimplex);
            tabControl1.Controls.Add(tabDuality);
            tabControl1.Controls.Add(tabBaB);
            tabControl1.Controls.Add(tabCuttingPlane);
            tabControl1.Controls.Add(tabSenAnalysis);
            tabControl1.Controls.Add(tabNLP);
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
            tabSimplex.Controls.Add(button3);
            tabSimplex.Controls.Add(btnExit);
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
            tabSimplex.Text = " Simplex Section";
            // 
            // button3
            // 
            button3.Location = new Point(1441, 802);
            button3.Name = "button3";
            button3.Size = new Size(135, 48);
            button3.TabIndex = 14;
            button3.Text = "Back";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // btnExit
            // 
            btnExit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(1582, 802);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(135, 48);
            btnExit.TabIndex = 13;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // tabDuality
            // 
            tabDuality.BackColor = SystemColors.ActiveCaption;
            tabDuality.Controls.Add(txtDualtiySaveLocation);
            tabDuality.Controls.Add(btnDualitySave);
            tabDuality.Controls.Add(label6);
            tabDuality.Controls.Add(rtbDualityOutput);
            tabDuality.Controls.Add(btnDualityBack);
            tabDuality.Controls.Add(btnDualityExit);
            tabDuality.Controls.Add(btnTestDuality);
            tabDuality.Location = new Point(4, 30);
            tabDuality.Name = "tabDuality";
            tabDuality.Size = new Size(1720, 853);
            tabDuality.TabIndex = 5;
            tabDuality.Text = "Duality Section";
            // 
            // txtDualtiySaveLocation
            // 
            txtDualtiySaveLocation.Location = new Point(456, 811);
            txtDualtiySaveLocation.Margin = new Padding(4, 3, 4, 3);
            txtDualtiySaveLocation.Name = "txtDualtiySaveLocation";
            txtDualtiySaveLocation.Size = new Size(382, 29);
            txtDualtiySaveLocation.TabIndex = 23;
            // 
            // btnDualitySave
            // 
            btnDualitySave.Location = new Point(254, 786);
            btnDualitySave.Margin = new Padding(4, 3, 4, 3);
            btnDualitySave.Name = "btnDualitySave";
            btnDualitySave.Size = new Size(194, 54);
            btnDualitySave.TabIndex = 22;
            btnDualitySave.Text = "Save Location";
            btnDualitySave.UseVisualStyleBackColor = true;
            btnDualitySave.Click += btnDualitySave_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label6.Location = new Point(254, 38);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(537, 29);
            label6.TabIndex = 21;
            label6.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // rtbDualityOutput
            // 
            rtbDualityOutput.Location = new Point(254, 73);
            rtbDualityOutput.Margin = new Padding(4, 3, 4, 3);
            rtbDualityOutput.Name = "rtbDualityOutput";
            rtbDualityOutput.Size = new Size(584, 707);
            rtbDualityOutput.TabIndex = 20;
            rtbDualityOutput.Text = "";
            // 
            // btnDualityBack
            // 
            btnDualityBack.Location = new Point(1440, 801);
            btnDualityBack.Name = "btnDualityBack";
            btnDualityBack.Size = new Size(135, 48);
            btnDualityBack.TabIndex = 19;
            btnDualityBack.Text = "Back";
            btnDualityBack.UseVisualStyleBackColor = true;
            btnDualityBack.Click += btnDualityBack_Click;
            // 
            // btnDualityExit
            // 
            btnDualityExit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDualityExit.Location = new Point(1581, 801);
            btnDualityExit.Name = "btnDualityExit";
            btnDualityExit.Size = new Size(135, 48);
            btnDualityExit.TabIndex = 18;
            btnDualityExit.Text = "Exit";
            btnDualityExit.UseVisualStyleBackColor = true;
            btnDualityExit.Click += btnDualityExit_Click;
            // 
            // btnTestDuality
            // 
            btnTestDuality.Location = new Point(7, 697);
            btnTestDuality.Name = "btnTestDuality";
            btnTestDuality.Size = new Size(240, 83);
            btnTestDuality.TabIndex = 1;
            btnTestDuality.Text = "Test Duality";
            btnTestDuality.UseVisualStyleBackColor = true;
            btnTestDuality.Click += btnTestDuality_Click;
            // 
            // tabBaB
            // 
            tabBaB.BackColor = SystemColors.ActiveCaption;
            tabBaB.Controls.Add(label1);
            tabBaB.Controls.Add(richTextBox1);
            tabBaB.Controls.Add(textBox1);
            tabBaB.Controls.Add(button6);
            tabBaB.Controls.Add(button4);
            tabBaB.Controls.Add(button1);
            tabBaB.Controls.Add(btnB_B);
            tabBaB.Controls.Add(btnB_B_B);
            tabBaB.Location = new Point(4, 30);
            tabBaB.Name = "tabBaB";
            tabBaB.Padding = new Padding(3);
            tabBaB.Size = new Size(1720, 853);
            tabBaB.TabIndex = 1;
            tabBaB.Text = "Branch And Bound Section";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label1.Location = new Point(284, 28);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(537, 29);
            label1.TabIndex = 19;
            label1.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(284, 60);
            richTextBox1.Margin = new Padding(4, 3, 4, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(584, 707);
            richTextBox1.TabIndex = 16;
            richTextBox1.Text = "";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(486, 805);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(382, 29);
            textBox1.TabIndex = 18;
            // 
            // button6
            // 
            button6.Location = new Point(284, 780);
            button6.Margin = new Padding(4, 3, 4, 3);
            button6.Name = "button6";
            button6.Size = new Size(194, 54);
            button6.TabIndex = 17;
            button6.Text = "Save Location";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1441, 802);
            button4.Name = "button4";
            button4.Size = new Size(135, 48);
            button4.TabIndex = 15;
            button4.Text = "Back";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.Location = new Point(1582, 802);
            button1.Name = "button1";
            button1.Size = new Size(135, 48);
            button1.TabIndex = 14;
            button1.Text = "Exit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabCuttingPlane
            // 
            tabCuttingPlane.BackColor = SystemColors.ActiveCaption;
            tabCuttingPlane.Controls.Add(label2);
            tabCuttingPlane.Controls.Add(cuttingPlaneTextbox);
            tabCuttingPlane.Controls.Add(textBox2);
            tabCuttingPlane.Controls.Add(button7);
            tabCuttingPlane.Controls.Add(button5);
            tabCuttingPlane.Controls.Add(button2);
            tabCuttingPlane.Controls.Add(btnCuttingPlane);
            tabCuttingPlane.Location = new Point(4, 30);
            tabCuttingPlane.Name = "tabCuttingPlane";
            tabCuttingPlane.Size = new Size(1720, 853);
            tabCuttingPlane.TabIndex = 2;
            tabCuttingPlane.Text = "Cutting Plane Section";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline, GraphicsUnit.Point, 0);
            label2.Location = new Point(280, 36);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(537, 29);
            label2.TabIndex = 19;
            label2.Text = "Preview LP Form; Canonical Form; Final Answer:\r\n";
            // 
            // cuttingPlaneTextbox
            // 
            cuttingPlaneTextbox.Location = new Point(280, 68);
            cuttingPlaneTextbox.Margin = new Padding(4, 3, 4, 3);
            cuttingPlaneTextbox.Name = "cuttingPlaneTextbox";
            cuttingPlaneTextbox.Size = new Size(584, 707);
            cuttingPlaneTextbox.TabIndex = 16;
            cuttingPlaneTextbox.Text = "";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(482, 813);
            textBox2.Margin = new Padding(4, 3, 4, 3);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(382, 29);
            textBox2.TabIndex = 18;
            // 
            // button7
            // 
            button7.Location = new Point(280, 788);
            button7.Margin = new Padding(4, 3, 4, 3);
            button7.Name = "button7";
            button7.Size = new Size(194, 54);
            button7.TabIndex = 17;
            button7.Text = "Save Location";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1441, 802);
            button5.Name = "button5";
            button5.Size = new Size(135, 48);
            button5.TabIndex = 15;
            button5.Text = "Back";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(1582, 802);
            button2.Name = "button2";
            button2.Size = new Size(135, 48);
            button2.TabIndex = 14;
            button2.Text = "Exit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabSenAnalysis
            // 
            tabSenAnalysis.BackColor = SystemColors.ActiveCaption;
            tabSenAnalysis.Controls.Add(txtAddResult);
            tabSenAnalysis.Controls.Add(label10);
            tabSenAnalysis.Controls.Add(btnAddActivity);
            tabSenAnalysis.Controls.Add(txtActivityValues);
            tabSenAnalysis.Controls.Add(btnAddConstraint);
            tabSenAnalysis.Controls.Add(txtRhs);
            tabSenAnalysis.Controls.Add(txtCoeff);
            tabSenAnalysis.Controls.Add(cbRelation);
            tabSenAnalysis.Controls.Add(label9);
            tabSenAnalysis.Controls.Add(label8);
            tabSenAnalysis.Controls.Add(label7);
            tabSenAnalysis.Controls.Add(btnSensitivity);
            tabSenAnalysis.Controls.Add(label5);
            tabSenAnalysis.Controls.Add(label4);
            tabSenAnalysis.Controls.Add(label3);
            tabSenAnalysis.Controls.Add(nudNewValue);
            tabSenAnalysis.Controls.Add(nudVarIndex);
            tabSenAnalysis.Controls.Add(rtbSensitivity);
            tabSenAnalysis.Location = new Point(4, 30);
            tabSenAnalysis.Name = "tabSenAnalysis";
            tabSenAnalysis.Size = new Size(1720, 853);
            tabSenAnalysis.TabIndex = 3;
            tabSenAnalysis.Text = "Sensitivity Analysis";
            // 
            // txtAddResult
            // 
            txtAddResult.Location = new Point(677, 332);
            txtAddResult.Name = "txtAddResult";
            txtAddResult.Size = new Size(735, 256);
            txtAddResult.TabIndex = 17;
            txtAddResult.Text = "";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(783, 187);
            label10.Name = "label10";
            label10.Size = new Size(149, 21);
            label10.TabIndex = 16;
            label10.Text = "One value per line";
            // 
            // btnAddActivity
            // 
            btnAddActivity.Location = new Point(677, 261);
            btnAddActivity.Name = "btnAddActivity";
            btnAddActivity.Size = new Size(136, 38);
            btnAddActivity.TabIndex = 15;
            btnAddActivity.Text = "AddActivity";
            btnAddActivity.UseVisualStyleBackColor = true;
            btnAddActivity.Click += btnAddActivity_Click;
            // 
            // txtActivityValues
            // 
            txtActivityValues.Location = new Point(677, 146);
            txtActivityValues.Name = "txtActivityValues";
            txtActivityValues.Size = new Size(100, 96);
            txtActivityValues.TabIndex = 14;
            txtActivityValues.Text = "";
            // 
            // btnAddConstraint
            // 
            btnAddConstraint.Location = new Point(676, 88);
            btnAddConstraint.Name = "btnAddConstraint";
            btnAddConstraint.Size = new Size(136, 38);
            btnAddConstraint.TabIndex = 13;
            btnAddConstraint.Text = "AddConstraint";
            btnAddConstraint.UseVisualStyleBackColor = true;
            btnAddConstraint.Click += btnAddConstraint_Click;
            // 
            // txtRhs
            // 
            txtRhs.Location = new Point(927, 38);
            txtRhs.Name = "txtRhs";
            txtRhs.Size = new Size(100, 29);
            txtRhs.TabIndex = 12;
            // 
            // txtCoeff
            // 
            txtCoeff.Location = new Point(677, 38);
            txtCoeff.Name = "txtCoeff";
            txtCoeff.Size = new Size(100, 29);
            txtCoeff.TabIndex = 11;
            // 
            // cbRelation
            // 
            cbRelation.FormattingEnabled = true;
            cbRelation.Items.AddRange(new object[] { "<=", ">=", "=" });
            cbRelation.Location = new Point(800, 38);
            cbRelation.Name = "cbRelation";
            cbRelation.Size = new Size(103, 29);
            cbRelation.TabIndex = 10;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(927, 14);
            label9.Name = "label9";
            label9.Size = new Size(37, 21);
            label9.TabIndex = 9;
            label9.Text = "Rhs";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(800, 14);
            label8.Name = "label8";
            label8.Size = new Size(74, 21);
            label8.TabIndex = 8;
            label8.Text = "Relation";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(676, 14);
            label7.Name = "label7";
            label7.Size = new Size(101, 21);
            label7.TabIndex = 7;
            label7.Text = "Coefficients";
            // 
            // btnSensitivity
            // 
            btnSensitivity.Location = new Point(314, 466);
            btnSensitivity.Name = "btnSensitivity";
            btnSensitivity.Size = new Size(161, 94);
            btnSensitivity.TabIndex = 6;
            btnSensitivity.Text = "Run Sensitivity Analysis";
            btnSensitivity.UseVisualStyleBackColor = true;
            btnSensitivity.Click += btnSensitivity_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 164);
            label5.Name = "label5";
            label5.Size = new Size(92, 21);
            label5.TabIndex = 5;
            label5.Text = "New Value";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 88);
            label4.Name = "label4";
            label4.Size = new Size(120, 21);
            label4.TabIndex = 4;
            label4.Text = "Variable Index";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Underline);
            label3.Location = new Point(198, 44);
            label3.Name = "label3";
            label3.Size = new Size(214, 29);
            label3.TabIndex = 3;
            label3.Text = "Sensitivity Analysis";
            // 
            // nudNewValue
            // 
            nudNewValue.Location = new Point(7, 188);
            nudNewValue.Name = "nudNewValue";
            nudNewValue.Size = new Size(120, 29);
            nudNewValue.TabIndex = 2;
            // 
            // nudVarIndex
            // 
            nudVarIndex.Location = new Point(7, 112);
            nudVarIndex.Name = "nudVarIndex";
            nudVarIndex.Size = new Size(120, 29);
            nudVarIndex.TabIndex = 1;
            // 
            // rtbSensitivity
            // 
            rtbSensitivity.Location = new Point(198, 76);
            rtbSensitivity.Name = "rtbSensitivity";
            rtbSensitivity.Size = new Size(397, 384);
            rtbSensitivity.TabIndex = 0;
            rtbSensitivity.Text = "";
            // 
            // tabNLP
            // 
            tabNLP.BackColor = SystemColors.ActiveCaption;
            tabNLP.Controls.Add(button8);
            tabNLP.Controls.Add(button9);
            tabNLP.Controls.Add(btnSolveNLP);
            tabNLP.Controls.Add(btnSaveNLPsolution);
            tabNLP.Controls.Add(rtbPreviewNLP);
            tabNLP.Controls.Add(txtDisplayFileLocationNLP);
            tabNLP.Controls.Add(btnLoadTextfileNLP);
            tabNLP.Location = new Point(4, 30);
            tabNLP.Name = "tabNLP";
            tabNLP.Size = new Size(1720, 853);
            tabNLP.TabIndex = 4;
            tabNLP.Text = "Non Linear Solver";
            // 
            // button8
            // 
            button8.Location = new Point(1440, 802);
            button8.Name = "button8";
            button8.Size = new Size(135, 48);
            button8.TabIndex = 17;
            button8.Text = "Back";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button9.Location = new Point(1581, 802);
            button9.Name = "button9";
            button9.Size = new Size(135, 48);
            button9.TabIndex = 16;
            button9.Text = "Exit";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // btnSolveNLP
            // 
            btnSolveNLP.Location = new Point(510, 775);
            btnSolveNLP.Name = "btnSolveNLP";
            btnSolveNLP.Size = new Size(166, 49);
            btnSolveNLP.TabIndex = 4;
            btnSolveNLP.Text = "Solve";
            btnSolveNLP.UseVisualStyleBackColor = true;
            btnSolveNLP.Click += btnSolveNLP_Click;
            // 
            // btnSaveNLPsolution
            // 
            btnSaveNLPsolution.Location = new Point(1128, 775);
            btnSaveNLPsolution.Name = "btnSaveNLPsolution";
            btnSaveNLPsolution.Size = new Size(166, 49);
            btnSaveNLPsolution.TabIndex = 3;
            btnSaveNLPsolution.Text = "Save Solution";
            btnSaveNLPsolution.UseVisualStyleBackColor = true;
            btnSaveNLPsolution.Click += btnSaveNLPsolution_Click;
            // 
            // rtbPreviewNLP
            // 
            rtbPreviewNLP.Location = new Point(510, 22);
            rtbPreviewNLP.Name = "rtbPreviewNLP";
            rtbPreviewNLP.Size = new Size(784, 747);
            rtbPreviewNLP.TabIndex = 2;
            rtbPreviewNLP.Text = "";
            rtbPreviewNLP.TextChanged += rtbPreviewNLP_TextChanged;
            // 
            // txtDisplayFileLocationNLP
            // 
            txtDisplayFileLocationNLP.Location = new Point(82, 215);
            txtDisplayFileLocationNLP.Name = "txtDisplayFileLocationNLP";
            txtDisplayFileLocationNLP.Size = new Size(281, 29);
            txtDisplayFileLocationNLP.TabIndex = 1;
            txtDisplayFileLocationNLP.TextChanged += txtDisplayFileLocationNLP_TextChanged;
            // 
            // btnLoadTextfileNLP
            // 
            btnLoadTextfileNLP.Location = new Point(82, 160);
            btnLoadTextfileNLP.Name = "btnLoadTextfileNLP";
            btnLoadTextfileNLP.Size = new Size(166, 49);
            btnLoadTextfileNLP.TabIndex = 0;
            btnLoadTextfileNLP.Text = "Load Textfile";
            btnLoadTextfileNLP.UseVisualStyleBackColor = true;
            btnLoadTextfileNLP.Click += btnLoadTextfileNLP_Click;
            // 
            // SolverForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1733, 891);
            Controls.Add(tabControl1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "SolverForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SolverForm";
            WindowState = FormWindowState.Maximized;
            Load += SolverForm_Load_1;
            tabControl1.ResumeLayout(false);
            tabSimplex.ResumeLayout(false);
            tabSimplex.PerformLayout();
            tabDuality.ResumeLayout(false);
            tabDuality.PerformLayout();
            tabBaB.ResumeLayout(false);
            tabBaB.PerformLayout();
            tabCuttingPlane.ResumeLayout(false);
            tabCuttingPlane.PerformLayout();
            tabSenAnalysis.ResumeLayout(false);
            tabSenAnalysis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudNewValue).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudVarIndex).EndInit();
            tabNLP.ResumeLayout(false);
            tabNLP.PerformLayout();
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
        private RichTextBox richTextBox1;
        private TextBox textBox1;
        private Button button6;
        private Label label2;
        private RichTextBox cuttingPlaneTextbox;
        private TextBox textBox2;
        private Button button7;
        private TabPage tabSenAnalysis;
        private TabPage tabNLP;
        private TextBox txtDisplayFileLocationNLP;
        private Button btnLoadTextfileNLP;
        private Button btnSaveNLPsolution;
        private RichTextBox rtbPreviewNLP;
        private Button btnSolveNLP;
        private Button button8;
        private Button button9;
        private Button btnSensitivity;
        private Label label5;
        private Label label4;
        private Label label3;
        private NumericUpDown nudNewValue;
        private NumericUpDown nudVarIndex;
        private RichTextBox rtbSensitivity;
        private TabPage tabDuality;
        private Button btnDualityBack;
        private Button btnDualityExit;
        private Button btnTestDuality;
        private TextBox txtDualtiySaveLocation;
        private Button btnDualitySave;
        private Label label6;
        private RichTextBox rtbDualityOutput;
        private TextBox txtRhs;
        private TextBox txtCoeff;
        private ComboBox cbRelation;
        private Label label9;
        private Label label8;
        private Label label7;
        private Button btnAddConstraint;
        private Label label10;
        private Button btnAddActivity;
        private RichTextBox txtActivityValues;
        private RichTextBox txtAddResult;
    }
}