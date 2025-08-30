namespace OperationsApp;

partial class startingForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new Label();
        btnUploadFile = new Button();
        btnContinue = new Button();
        btnExit = new Button();
        txtFileOutput = new TextBox();
        rtbOutput = new RichTextBox();
        SuspendLayout();
        // 
        // label1
        // 
        label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        label1.AutoSize = true;
        label1.Font = new Font("Segoe UI", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
        label1.Location = new Point(2, 9);
        label1.Name = "label1";
        label1.Size = new Size(904, 86);
        label1.TabIndex = 0;
        label1.Text = "**********LinearPro**********";
        // 
        // btnUploadFile
        // 
        btnUploadFile.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnUploadFile.Location = new Point(326, 98);
        btnUploadFile.Name = "btnUploadFile";
        btnUploadFile.Size = new Size(239, 67);
        btnUploadFile.TabIndex = 1;
        btnUploadFile.Text = "Upload File";
        btnUploadFile.UseVisualStyleBackColor = true;
        btnUploadFile.Click += btnUploadFile_Click;
        // 
        // btnContinue
        // 
        btnContinue.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnContinue.Location = new Point(606, 418);
        btnContinue.Name = "btnContinue";
        btnContinue.Size = new Size(135, 48);
        btnContinue.TabIndex = 2;
        btnContinue.Text = "Continue";
        btnContinue.UseVisualStyleBackColor = true;
        btnContinue.Click += btnContinue_Click;
        // 
        // btnExit
        // 
        btnExit.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
        btnExit.Location = new Point(747, 418);
        btnExit.Name = "btnExit";
        btnExit.Size = new Size(135, 48);
        btnExit.TabIndex = 3;
        btnExit.Text = "Exit";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += btnExit_Click;
        // 
        // txtFileOutput
        // 
        txtFileOutput.Location = new Point(234, 171);
        txtFileOutput.Name = "txtFileOutput";
        txtFileOutput.Size = new Size(435, 23);
        txtFileOutput.TabIndex = 4;
        // 
        // rtbOutput
        // 
        rtbOutput.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        rtbOutput.Location = new Point(310, 200);
        rtbOutput.Name = "rtbOutput";
        rtbOutput.Size = new Size(268, 119);
        rtbOutput.TabIndex = 5;
        rtbOutput.Text = "";
        // 
        // startingForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.ActiveCaption;
        ClientSize = new Size(894, 477);
        Controls.Add(rtbOutput);
        Controls.Add(txtFileOutput);
        Controls.Add(btnExit);
        Controls.Add(btnContinue);
        Controls.Add(btnUploadFile);
        Controls.Add(label1);
        Name = "startingForm";
        Text = "Welcome to LinearPro";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button btnUploadFile;
    private Button btnContinue;
    private Button btnExit;
    private TextBox txtFileOutput;
    private RichTextBox rtbOutput;
}