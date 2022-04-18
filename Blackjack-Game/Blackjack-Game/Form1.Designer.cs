﻿namespace Blackjack_Game
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnPlaySingle = new System.Windows.Forms.Button();
            this.btnJoin = new System.Windows.Forms.Button();
            this.pnlChat = new System.Windows.Forms.Panel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.lblIncChallenge = new System.Windows.Forms.Label();
            this.btnIssue = new System.Windows.Forms.Button();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbUsers = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txtServIP = new System.Windows.Forms.TextBox();
            this.lblServIP = new System.Windows.Forms.Label();
            this.btnDecline = new System.Windows.Forms.Button();
            this.pnlChat.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlaySingle
            // 
            this.btnPlaySingle.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPlaySingle.Location = new System.Drawing.Point(319, 481);
            this.btnPlaySingle.Name = "btnPlaySingle";
            this.btnPlaySingle.Size = new System.Drawing.Size(450, 54);
            this.btnPlaySingle.TabIndex = 0;
            this.btnPlaySingle.Text = "Play Against AI";
            this.btnPlaySingle.UseVisualStyleBackColor = true;
            this.btnPlaySingle.Click += new System.EventHandler(this.btnPlaySingle_Click);
            // 
            // btnJoin
            // 
            this.btnJoin.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnJoin.Location = new System.Drawing.Point(319, 541);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(450, 54);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "Join Chat / Find Game";
            this.btnJoin.UseVisualStyleBackColor = true;
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // pnlChat
            // 
            this.pnlChat.BackColor = System.Drawing.Color.Black;
            this.pnlChat.Controls.Add(this.btnDecline);
            this.pnlChat.Controls.Add(this.btnAccept);
            this.pnlChat.Controls.Add(this.lblIncChallenge);
            this.pnlChat.Controls.Add(this.btnIssue);
            this.pnlChat.Controls.Add(this.txtUsername);
            this.pnlChat.Controls.Add(this.label2);
            this.pnlChat.Controls.Add(this.cmbUsers);
            this.pnlChat.Controls.Add(this.label1);
            this.pnlChat.Controls.Add(this.btnSendMessage);
            this.pnlChat.Controls.Add(this.btnStart);
            this.pnlChat.Controls.Add(this.txtMessage);
            this.pnlChat.Controls.Add(this.listBox1);
            this.pnlChat.Controls.Add(this.txtServIP);
            this.pnlChat.Controls.Add(this.lblServIP);
            this.pnlChat.ForeColor = System.Drawing.Color.White;
            this.pnlChat.Location = new System.Drawing.Point(12, 12);
            this.pnlChat.Name = "pnlChat";
            this.pnlChat.Size = new System.Drawing.Size(1060, 597);
            this.pnlChat.TabIndex = 3;
            // 
            // btnAccept
            // 
            this.btnAccept.BackColor = System.Drawing.Color.Black;
            this.btnAccept.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnAccept.Location = new System.Drawing.Point(833, 548);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(209, 29);
            this.btnAccept.TabIndex = 12;
            this.btnAccept.Text = "Accept Challenge";
            this.btnAccept.UseVisualStyleBackColor = false;
            // 
            // lblIncChallenge
            // 
            this.lblIncChallenge.AutoSize = true;
            this.lblIncChallenge.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblIncChallenge.ForeColor = System.Drawing.Color.Red;
            this.lblIncChallenge.Location = new System.Drawing.Point(391, 552);
            this.lblIncChallenge.Name = "lblIncChallenge";
            this.lblIncChallenge.Size = new System.Drawing.Size(287, 21);
            this.lblIncChallenge.TabIndex = 11;
            this.lblIncChallenge.Text = "Received Challenge From XXXXXXXX";
            // 
            // btnIssue
            // 
            this.btnIssue.BackColor = System.Drawing.Color.Black;
            this.btnIssue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnIssue.Location = new System.Drawing.Point(833, 494);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(209, 30);
            this.btnIssue.TabIndex = 10;
            this.btnIssue.Text = "Issue Challenge";
            this.btnIssue.UseVisualStyleBackColor = false;
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.Black;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location = new System.Drawing.Point(138, 4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderText = " Max 8 Characters";
            this.txtUsername.Size = new System.Drawing.Size(282, 29);
            this.txtUsername.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(17, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "Enter Username:";
            // 
            // cmbUsers
            // 
            this.cmbUsers.BackColor = System.Drawing.Color.Black;
            this.cmbUsers.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbUsers.ForeColor = System.Drawing.Color.White;
            this.cmbUsers.FormattingEnabled = true;
            this.cmbUsers.Location = new System.Drawing.Point(607, 495);
            this.cmbUsers.Name = "cmbUsers";
            this.cmbUsers.Size = new System.Drawing.Size(220, 29);
            this.cmbUsers.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(391, 499);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "Challenge User to a Game:";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.BackColor = System.Drawing.Color.Black;
            this.btnSendMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSendMessage.Location = new System.Drawing.Point(833, 454);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(209, 29);
            this.btnSendMessage.TabIndex = 5;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Black;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnStart.Location = new System.Drawing.Point(885, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(157, 29);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Join Server";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.Black;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtMessage.ForeColor = System.Drawing.Color.White;
            this.txtMessage.Location = new System.Drawing.Point(14, 454);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.PlaceholderText = " Your message here...";
            this.txtMessage.Size = new System.Drawing.Size(813, 29);
            this.txtMessage.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Black;
            this.listBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 21;
            this.listBox1.Location = new System.Drawing.Point(14, 56);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1028, 382);
            this.listBox1.TabIndex = 2;
            // 
            // txtServIP
            // 
            this.txtServIP.BackColor = System.Drawing.Color.Black;
            this.txtServIP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtServIP.ForeColor = System.Drawing.Color.White;
            this.txtServIP.Location = new System.Drawing.Point(588, 3);
            this.txtServIP.Name = "txtServIP";
            this.txtServIP.Size = new System.Drawing.Size(282, 29);
            this.txtServIP.TabIndex = 1;
            // 
            // lblServIP
            // 
            this.lblServIP.AutoSize = true;
            this.lblServIP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblServIP.ForeColor = System.Drawing.Color.White;
            this.lblServIP.Location = new System.Drawing.Point(467, 6);
            this.lblServIP.Name = "lblServIP";
            this.lblServIP.Size = new System.Drawing.Size(115, 21);
            this.lblServIP.TabIndex = 0;
            this.lblServIP.Text = "Enter Server IP:";
            // 
            // btnDecline
            // 
            this.btnDecline.BackColor = System.Drawing.Color.Black;
            this.btnDecline.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDecline.Location = new System.Drawing.Point(684, 548);
            this.btnDecline.Name = "btnDecline";
            this.btnDecline.Size = new System.Drawing.Size(143, 29);
            this.btnDecline.TabIndex = 13;
            this.btnDecline.Text = "Decline";
            this.btnDecline.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1084, 621);
            this.Controls.Add(this.pnlChat);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnPlaySingle);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blackjack";
            this.pnlChat.ResumeLayout(false);
            this.pnlChat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnPlaySingle;
        private Button btnJoin;
        private Panel pnlChat;
        private TextBox txtMessage;
        private ListBox listBox1;
        private TextBox txtServIP;
        private Label lblServIP;
        private Button btnSendMessage;
        private Button btnStart;
        private Button btnAccept;
        private Label lblIncChallenge;
        private Button btnIssue;
        private TextBox txtUsername;
        private Label label2;
        private ComboBox cmbUsers;
        private Label label1;
        private Button btnDecline;
    }
}