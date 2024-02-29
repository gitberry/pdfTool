
namespace pdfTool
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnExecute = new System.Windows.Forms.Button();
            this.cboAction = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.chkVerbose = new System.Windows.Forms.CheckBox();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.chkSafetyOverwrite = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Enabled = false;
            this.btnExecute.Location = new System.Drawing.Point(12, 12);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(267, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Do It!";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // cboAction
            // 
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.FormattingEnabled = true;
            this.cboAction.Location = new System.Drawing.Point(285, 12);
            this.cboAction.Name = "cboAction";
            this.cboAction.Size = new System.Drawing.Size(438, 21);
            this.cboAction.TabIndex = 1;
            this.cboAction.SelectedIndexChanged += new System.EventHandler(this.cboAction_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(285, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(728, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(285, 66);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(728, 20);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(285, 92);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(728, 20);
            this.textBox3.TabIndex = 6;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.Location = new System.Drawing.Point(285, 118);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(728, 20);
            this.textBox4.TabIndex = 8;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.Location = new System.Drawing.Point(285, 144);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(728, 20);
            this.textBox5.TabIndex = 10;
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.Location = new System.Drawing.Point(12, 170);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsole.Size = new System.Drawing.Size(1001, 327);
            this.txtConsole.TabIndex = 12;
            // 
            // chkVerbose
            // 
            this.chkVerbose.AutoSize = true;
            this.chkVerbose.Checked = true;
            this.chkVerbose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVerbose.Enabled = false;
            this.chkVerbose.Location = new System.Drawing.Point(729, 16);
            this.chkVerbose.Name = "chkVerbose";
            this.chkVerbose.Size = new System.Drawing.Size(65, 17);
            this.chkVerbose.TabIndex = 13;
            this.chkVerbose.Text = "Verbose";
            this.chkVerbose.UseVisualStyleBackColor = true;
            // 
            // chkLog
            // 
            this.chkLog.AutoSize = true;
            this.chkLog.Enabled = false;
            this.chkLog.Location = new System.Drawing.Point(800, 16);
            this.chkLog.Name = "chkLog";
            this.chkLog.Size = new System.Drawing.Size(44, 17);
            this.chkLog.TabIndex = 14;
            this.chkLog.Text = "Log";
            this.chkLog.UseVisualStyleBackColor = true;
            // 
            // chkSafetyOverwrite
            // 
            this.chkSafetyOverwrite.AutoSize = true;
            this.chkSafetyOverwrite.Location = new System.Drawing.Point(850, 16);
            this.chkSafetyOverwrite.Name = "chkSafetyOverwrite";
            this.chkSafetyOverwrite.Size = new System.Drawing.Size(163, 17);
            this.chkSafetyOverwrite.TabIndex = 15;
            this.chkSafetyOverwrite.Text = "Rename existing Output Files";
            this.chkSafetyOverwrite.UseVisualStyleBackColor = true;
            this.chkSafetyOverwrite.CheckedChanged += new System.EventHandler(this.chkSafetyOverwrite_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 13);
            this.label1.TabIndex = 16;
            this.label1.TabStop = false;
            this.label1.Text = "label1:";
            this.label1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label1.Click += new System.EventHandler(this.labeldefocus);
            // 
            // label2
            // 
            this.label2.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label2.Location = new System.Drawing.Point(12, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(267, 13);
            this.label2.TabIndex = 17;
            this.label2.TabStop = false;
            this.label2.Text = "label2:";
            this.label2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label2.Click += new System.EventHandler(this.labeldefocus);
            // 
            // label3
            // 
            this.label3.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(267, 13);
            this.label3.TabIndex = 18;
            this.label3.TabStop = false;
            this.label3.Text = "label3:";
            this.label3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label3.Click += new System.EventHandler(this.labeldefocus);
            // 
            // label4
            // 
            this.label4.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label4.Location = new System.Drawing.Point(12, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(267, 13);
            this.label4.TabIndex = 19;
            this.label4.TabStop = false;
            this.label4.Text = "label4:";
            this.label4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label4.Click += new System.EventHandler(this.labeldefocus);
            // 
            // label5
            // 
            this.label5.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.label5.Location = new System.Drawing.Point(12, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(267, 13);
            this.label5.TabIndex = 20;
            this.label5.TabStop = false;
            this.label5.Text = "label5:";
            this.label5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.label5.Click += new System.EventHandler(this.labeldefocus);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 509);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSafetyOverwrite);
            this.Controls.Add(this.chkLog);
            this.Controls.Add(this.chkVerbose);
            this.Controls.Add(this.txtConsole);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cboAction);
            this.Controls.Add(this.btnExecute);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "pdf tool";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.ComboBox cboAction;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.CheckBox chkVerbose;
        private System.Windows.Forms.CheckBox chkLog;
        private System.Windows.Forms.CheckBox chkSafetyOverwrite;
        private System.Windows.Forms.TextBox label1;
        private System.Windows.Forms.TextBox label2;
        private System.Windows.Forms.TextBox label3;
        private System.Windows.Forms.TextBox label4;
        private System.Windows.Forms.TextBox label5;
    }
}

