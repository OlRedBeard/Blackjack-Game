namespace Blackjack_Game
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
            this.btnHost = new System.Windows.Forms.Button();
            this.btnJoin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlaySingle
            // 
            this.btnPlaySingle.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPlaySingle.Location = new System.Drawing.Point(85, 481);
            this.btnPlaySingle.Name = "btnPlaySingle";
            this.btnPlaySingle.Size = new System.Drawing.Size(450, 54);
            this.btnPlaySingle.TabIndex = 0;
            this.btnPlaySingle.Text = "Play Against AI";
            this.btnPlaySingle.UseVisualStyleBackColor = true;
            this.btnPlaySingle.Click += new System.EventHandler(this.btnPlaySingle_Click);
            // 
            // btnHost
            // 
            this.btnHost.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnHost.Location = new System.Drawing.Point(541, 481);
            this.btnHost.Name = "btnHost";
            this.btnHost.Size = new System.Drawing.Size(450, 54);
            this.btnHost.TabIndex = 1;
            this.btnHost.Text = "Host Online Game";
            this.btnHost.UseVisualStyleBackColor = true;
            // 
            // btnJoin
            // 
            this.btnJoin.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnJoin.Location = new System.Drawing.Point(541, 541);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(450, 54);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "Join Online Game";
            this.btnJoin.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1084, 621);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.btnHost);
            this.Controls.Add(this.btnPlaySingle);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blackjack";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnPlaySingle;
        private Button btnHost;
        private Button btnJoin;
    }
}