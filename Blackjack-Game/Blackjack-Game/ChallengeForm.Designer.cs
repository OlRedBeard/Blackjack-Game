namespace Blackjack_Game
{
    partial class ChallengeForm
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
            this.cmbIP = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblContext = new System.Windows.Forms.Label();
            this.btnChallenge = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbIP
            // 
            this.cmbIP.BackColor = System.Drawing.Color.Black;
            this.cmbIP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbIP.ForeColor = System.Drawing.Color.White;
            this.cmbIP.FormattingEnabled = true;
            this.cmbIP.Location = new System.Drawing.Point(140, 12);
            this.cmbIP.Name = "cmbIP";
            this.cmbIP.Size = new System.Drawing.Size(242, 29);
            this.cmbIP.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Your IP Address:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblContext
            // 
            this.lblContext.BackColor = System.Drawing.Color.Black;
            this.lblContext.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblContext.ForeColor = System.Drawing.Color.Gold;
            this.lblContext.Location = new System.Drawing.Point(16, 55);
            this.lblContext.Name = "lblContext";
            this.lblContext.Size = new System.Drawing.Size(366, 36);
            this.lblContext.TabIndex = 2;
            this.lblContext.Text = "Send Challenge to XXXXXXXX";
            this.lblContext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnChallenge
            // 
            this.btnChallenge.BackColor = System.Drawing.Color.Black;
            this.btnChallenge.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnChallenge.ForeColor = System.Drawing.Color.White;
            this.btnChallenge.Location = new System.Drawing.Point(202, 103);
            this.btnChallenge.Name = "btnChallenge";
            this.btnChallenge.Size = new System.Drawing.Size(180, 35);
            this.btnChallenge.TabIndex = 3;
            this.btnChallenge.Text = "Send Challenge";
            this.btnChallenge.UseVisualStyleBackColor = false;
            this.btnChallenge.Click += new System.EventHandler(this.btnChallenge_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Black;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(16, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ChallengeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(397, 157);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChallenge);
            this.Controls.Add(this.lblContext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbIP);
            this.Name = "ChallengeForm";
            this.Text = "PvP Challenge";
            this.Load += new System.EventHandler(this.ChallengeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox cmbIP;
        private Label label1;
        private Label lblContext;
        private Button btnChallenge;
        private Button btnCancel;
    }
}