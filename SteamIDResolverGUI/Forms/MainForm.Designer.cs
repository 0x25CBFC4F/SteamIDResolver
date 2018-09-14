namespace SteamIDResolverGUI.Forms
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.loginTextBox = new System.Windows.Forms.TextBox();
            this.resolveLoginButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.steamID32Link = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.steamID64Link = new System.Windows.Forms.LinkLabel();
            this.steamIDLink = new System.Windows.Forms.LinkLabel();
            this.steamRepLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter steam login:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loginTextBox
            // 
            this.loginTextBox.BackColor = System.Drawing.Color.Black;
            this.loginTextBox.ForeColor = System.Drawing.Color.White;
            this.loginTextBox.Location = new System.Drawing.Point(12, 32);
            this.loginTextBox.Name = "loginTextBox";
            this.loginTextBox.Size = new System.Drawing.Size(223, 20);
            this.loginTextBox.TabIndex = 1;
            // 
            // resolveLoginButton
            // 
            this.resolveLoginButton.ForeColor = System.Drawing.Color.Black;
            this.resolveLoginButton.Location = new System.Drawing.Point(12, 63);
            this.resolveLoginButton.Name = "resolveLoginButton";
            this.resolveLoginButton.Size = new System.Drawing.Size(223, 23);
            this.resolveLoginButton.TabIndex = 2;
            this.resolveLoginButton.Text = "resolve!";
            this.resolveLoginButton.UseVisualStyleBackColor = true;
            this.resolveLoginButton.Click += new System.EventHandler(this.resolveLoginButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Resolving results:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "SteamID32:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // steamID32Link
            // 
            this.steamID32Link.AutoSize = true;
            this.steamID32Link.LinkColor = System.Drawing.Color.Aqua;
            this.steamID32Link.Location = new System.Drawing.Point(73, 127);
            this.steamID32Link.Name = "steamID32Link";
            this.steamID32Link.Size = new System.Drawing.Size(82, 13);
            this.steamID32Link.TabIndex = 6;
            this.steamID32Link.TabStop = true;
            this.steamID32Link.Text = "< steam ID 32 >";
            this.steamID32Link.VisitedLinkColor = System.Drawing.Color.Aqua;
            this.steamID32Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamID32Link_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "SteamID64:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // steamID64Link
            // 
            this.steamID64Link.AutoSize = true;
            this.steamID64Link.LinkColor = System.Drawing.Color.Aqua;
            this.steamID64Link.Location = new System.Drawing.Point(73, 151);
            this.steamID64Link.Name = "steamID64Link";
            this.steamID64Link.Size = new System.Drawing.Size(82, 13);
            this.steamID64Link.TabIndex = 8;
            this.steamID64Link.TabStop = true;
            this.steamID64Link.Text = "< steam ID 64 >";
            this.steamID64Link.VisitedLinkColor = System.Drawing.Color.Aqua;
            this.steamID64Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamID64Link_LinkClicked);
            // 
            // steamIDLink
            // 
            this.steamIDLink.LinkColor = System.Drawing.Color.Aqua;
            this.steamIDLink.Location = new System.Drawing.Point(12, 185);
            this.steamIDLink.Name = "steamIDLink";
            this.steamIDLink.Size = new System.Drawing.Size(226, 16);
            this.steamIDLink.TabIndex = 9;
            this.steamIDLink.TabStop = true;
            this.steamIDLink.Text = "SteamID.io";
            this.steamIDLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.steamIDLink.VisitedLinkColor = System.Drawing.Color.Aqua;
            this.steamIDLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamIDLink_LinkClicked);
            // 
            // steamRepLink
            // 
            this.steamRepLink.LinkColor = System.Drawing.Color.Aqua;
            this.steamRepLink.Location = new System.Drawing.Point(12, 201);
            this.steamRepLink.Name = "steamRepLink";
            this.steamRepLink.Size = new System.Drawing.Size(226, 17);
            this.steamRepLink.TabIndex = 10;
            this.steamRepLink.TabStop = true;
            this.steamRepLink.Text = "SteamREP.com";
            this.steamRepLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.steamRepLink.VisitedLinkColor = System.Drawing.Color.Aqua;
            this.steamRepLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.steamRepLink_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(250, 94);
            this.Controls.Add(this.steamRepLink);
            this.Controls.Add(this.steamIDLink);
            this.Controls.Add(this.steamID64Link);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.steamID32Link);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.resolveLoginButton);
            this.Controls.Add(this.loginTextBox);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Steam ID Resolver";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox loginTextBox;
        private System.Windows.Forms.Button resolveLoginButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel steamID32Link;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel steamID64Link;
        private System.Windows.Forms.LinkLabel steamIDLink;
        private System.Windows.Forms.LinkLabel steamRepLink;
    }
}

