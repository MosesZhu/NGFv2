namespace NGF.Tools.DatabaseCreater
{
    partial class DatabaseCreaterForm
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
            this.lblConnectString = new System.Windows.Forms.Label();
            this.tbxConnectString = new System.Windows.Forms.TextBox();
            this.tbxLoginUrl = new System.Windows.Forms.TextBox();
            this.lblLoginUrl = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblConnectString
            // 
            this.lblConnectString.AutoSize = true;
            this.lblConnectString.Location = new System.Drawing.Point(13, 12);
            this.lblConnectString.Name = "lblConnectString";
            this.lblConnectString.Size = new System.Drawing.Size(65, 12);
            this.lblConnectString.TabIndex = 0;
            this.lblConnectString.Text = "连接字符串";
            // 
            // tbxConnectString
            // 
            this.tbxConnectString.Location = new System.Drawing.Point(12, 35);
            this.tbxConnectString.Name = "tbxConnectString";
            this.tbxConnectString.Size = new System.Drawing.Size(609, 21);
            this.tbxConnectString.TabIndex = 1;
            // 
            // tbxLoginUrl
            // 
            this.tbxLoginUrl.Location = new System.Drawing.Point(12, 102);
            this.tbxLoginUrl.Name = "tbxLoginUrl";
            this.tbxLoginUrl.Size = new System.Drawing.Size(609, 21);
            this.tbxLoginUrl.TabIndex = 3;
            // 
            // lblLoginUrl
            // 
            this.lblLoginUrl.AutoSize = true;
            this.lblLoginUrl.Location = new System.Drawing.Point(13, 79);
            this.lblLoginUrl.Name = "lblLoginUrl";
            this.lblLoginUrl.Size = new System.Drawing.Size(59, 12);
            this.lblLoginUrl.TabIndex = 2;
            this.lblLoginUrl.Text = "登录页Url";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(546, 145);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tbxLog
            // 
            this.tbxLog.Location = new System.Drawing.Point(12, 188);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.Size = new System.Drawing.Size(609, 190);
            this.tbxLog.TabIndex = 5;
            // 
            // DatabaseCreaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 390);
            this.Controls.Add(this.tbxLog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbxLoginUrl);
            this.Controls.Add(this.lblLoginUrl);
            this.Controls.Add(this.tbxConnectString);
            this.Controls.Add(this.lblConnectString);
            this.MaximizeBox = false;
            this.Name = "DatabaseCreaterForm";
            this.Text = "DatabaseCreaterForm";
            this.Load += new System.EventHandler(this.DatabaseCreaterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConnectString;
        private System.Windows.Forms.TextBox tbxConnectString;
        private System.Windows.Forms.TextBox tbxLoginUrl;
        private System.Windows.Forms.Label lblLoginUrl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbxLog;
    }
}