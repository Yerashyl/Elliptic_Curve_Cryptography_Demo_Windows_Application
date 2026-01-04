namespace ECC_Demo
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabNetworkChat = new System.Windows.Forms.TabPage();
            this.grpMessaging = new System.Windows.Forms.GroupBox();
            this.btnEncryptSend = new System.Windows.Forms.Button();
            this.txtMessageInput = new System.Windows.Forms.TextBox();
            this.rtbChatLog = new System.Windows.Forms.RichTextBox();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtTargetPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.txtMyPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabFileNotary = new System.Windows.Forms.TabPage();
            this.grpNotaryInfo = new System.Windows.Forms.GroupBox();
            this.lblNotaryStatus = new System.Windows.Forms.Label();
            this.btnVerifyFile = new System.Windows.Forms.Button();
            this.btnSignFile = new System.Windows.Forms.Button();
            this.rtbNotaryLog = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabNetworkChat.SuspendLayout();
            this.grpMessaging.SuspendLayout();
            this.grpConnection.SuspendLayout();
            this.tabFileNotary.SuspendLayout();
            this.grpNotaryInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNetworkChat);
            this.tabControl1.Controls.Add(this.tabFileNotary);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabNetworkChat
            // 
            this.tabNetworkChat.Controls.Add(this.grpMessaging);
            this.tabNetworkChat.Controls.Add(this.rtbChatLog);
            this.tabNetworkChat.Controls.Add(this.grpConnection);
            this.tabNetworkChat.Location = new System.Drawing.Point(4, 24);
            this.tabNetworkChat.Name = "tabNetworkChat";
            this.tabNetworkChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabNetworkChat.Size = new System.Drawing.Size(792, 572);
            this.tabNetworkChat.TabIndex = 0;
            this.tabNetworkChat.Text = "Network Chat (ECDHE + ECIES)";
            this.tabNetworkChat.UseVisualStyleBackColor = true;
            // 
            // grpMessaging
            // 
            this.grpMessaging.Controls.Add(this.btnEncryptSend);
            this.grpMessaging.Controls.Add(this.txtMessageInput);
            this.grpMessaging.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpMessaging.Location = new System.Drawing.Point(3, 499);
            this.grpMessaging.Name = "grpMessaging";
            this.grpMessaging.Size = new System.Drawing.Size(786, 70);
            this.grpMessaging.TabIndex = 2;
            this.grpMessaging.TabStop = false;
            this.grpMessaging.Text = "Message Input";
            // 
            // btnEncryptSend
            // 
            this.btnEncryptSend.Location = new System.Drawing.Point(650, 22);
            this.btnEncryptSend.Name = "btnEncryptSend";
            this.btnEncryptSend.Size = new System.Drawing.Size(120, 30);
            this.btnEncryptSend.TabIndex = 1;
            this.btnEncryptSend.Text = "Encrypt && Send";
            this.btnEncryptSend.UseVisualStyleBackColor = true;
            this.btnEncryptSend.Click += new System.EventHandler(this.btnEncryptSend_Click);
            // 
            // txtMessageInput
            // 
            this.txtMessageInput.Location = new System.Drawing.Point(10, 25);
            this.txtMessageInput.Name = "txtMessageInput";
            this.txtMessageInput.Size = new System.Drawing.Size(630, 23);
            this.txtMessageInput.TabIndex = 0;
            // 
            // rtbChatLog
            // 
            this.rtbChatLog.BackColor = System.Drawing.Color.Black;
            this.rtbChatLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbChatLog.ForeColor = System.Drawing.Color.Lime;
            this.rtbChatLog.Location = new System.Drawing.Point(3, 83);
            this.rtbChatLog.Name = "rtbChatLog";
            this.rtbChatLog.ReadOnly = true;
            this.rtbChatLog.Size = new System.Drawing.Size(786, 486);
            this.rtbChatLog.TabIndex = 1;
            this.rtbChatLog.Text = "";
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.btnConnect);
            this.grpConnection.Controls.Add(this.txtTargetPort);
            this.grpConnection.Controls.Add(this.label2);
            this.grpConnection.Controls.Add(this.btnStartServer);
            this.grpConnection.Controls.Add(this.txtMyPort);
            this.grpConnection.Controls.Add(this.label1);
            this.grpConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpConnection.Location = new System.Drawing.Point(3, 3);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(786, 80);
            this.grpConnection.TabIndex = 0;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "Peer-to-Peer Connection";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(550, 30);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect to Peer";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtTargetPort
            // 
            this.txtTargetPort.Location = new System.Drawing.Point(440, 30);
            this.txtTargetPort.Name = "txtTargetPort";
            this.txtTargetPort.Size = new System.Drawing.Size(100, 23);
            this.txtTargetPort.TabIndex = 4;
            this.txtTargetPort.Text = "5001";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Target Port:";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(180, 30);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(120, 23);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // txtMyPort
            // 
            this.txtMyPort.Location = new System.Drawing.Point(70, 30);
            this.txtMyPort.Name = "txtMyPort";
            this.txtMyPort.Size = new System.Drawing.Size(100, 23);
            this.txtMyPort.TabIndex = 1;
            this.txtMyPort.Text = "5000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "My Port:";
            // 
            // tabFileNotary
            // 
            this.tabFileNotary.Controls.Add(this.grpNotaryInfo);
            this.tabFileNotary.Controls.Add(this.btnVerifyFile);
            this.tabFileNotary.Controls.Add(this.btnSignFile);
            this.tabFileNotary.Controls.Add(this.rtbNotaryLog);
            this.tabFileNotary.Location = new System.Drawing.Point(4, 24);
            this.tabFileNotary.Name = "tabFileNotary";
            this.tabFileNotary.Padding = new System.Windows.Forms.Padding(3);
            this.tabFileNotary.Size = new System.Drawing.Size(792, 572);
            this.tabFileNotary.TabIndex = 1;
            this.tabFileNotary.Text = "File Notary (ECDSA)";
            this.tabFileNotary.UseVisualStyleBackColor = true;
            // 
            // grpNotaryInfo
            // 
            this.grpNotaryInfo.Controls.Add(this.lblNotaryStatus);
            this.grpNotaryInfo.Location = new System.Drawing.Point(20, 150);
            this.grpNotaryInfo.Name = "grpNotaryInfo";
            this.grpNotaryInfo.Size = new System.Drawing.Size(750, 100);
            this.grpNotaryInfo.TabIndex = 2;
            this.grpNotaryInfo.TabStop = false;
            this.grpNotaryInfo.Text = "Status";
            // 
            // lblNotaryStatus
            // 
            this.lblNotaryStatus.AutoSize = true;
            this.lblNotaryStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNotaryStatus.Location = new System.Drawing.Point(20, 40);
            this.lblNotaryStatus.Name = "lblNotaryStatus";
            this.lblNotaryStatus.Size = new System.Drawing.Size(94, 21);
            this.lblNotaryStatus.TabIndex = 0;
            this.lblNotaryStatus.Text = "Select Action";
            // 
            // btnVerifyFile
            // 
            this.btnVerifyFile.Location = new System.Drawing.Point(20, 80);
            this.btnVerifyFile.Name = "btnVerifyFile";
            this.btnVerifyFile.Size = new System.Drawing.Size(200, 40);
            this.btnVerifyFile.TabIndex = 1;
            this.btnVerifyFile.Text = "Verify File Signature";
            this.btnVerifyFile.UseVisualStyleBackColor = true;
            this.btnVerifyFile.Click += new System.EventHandler(this.btnVerifyFile_Click);
            // 
            // btnSignFile
            // 
            this.btnSignFile.Location = new System.Drawing.Point(20, 30);
            this.btnSignFile.Name = "btnSignFile";
            this.btnSignFile.Size = new System.Drawing.Size(200, 40);
            this.btnSignFile.TabIndex = 0;
            this.btnSignFile.Text = "Sign File (Create .sig && .pub)";
            this.btnSignFile.UseVisualStyleBackColor = true;
            this.btnSignFile.Click += new System.EventHandler(this.btnSignFile_Click);
            // 
            // rtbNotaryLog
            // 
            this.rtbNotaryLog.BackColor = System.Drawing.Color.Black;
            this.rtbNotaryLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbNotaryLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbNotaryLog.ForeColor = System.Drawing.Color.Lime;
            this.rtbNotaryLog.Location = new System.Drawing.Point(3, 260); // Adjust Y as needed, or use Dock
            this.rtbNotaryLog.Name = "rtbNotaryLog";
            this.rtbNotaryLog.ReadOnly = true;
            this.rtbNotaryLog.Size = new System.Drawing.Size(786, 309);
            this.rtbNotaryLog.TabIndex = 3;
            this.rtbNotaryLog.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "ECC Crypto-Dashboard";
            this.tabControl1.ResumeLayout(false);
            this.tabNetworkChat.ResumeLayout(false);
            this.grpMessaging.ResumeLayout(false);
            this.grpMessaging.PerformLayout();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.tabFileNotary.ResumeLayout(false);
            this.grpNotaryInfo.ResumeLayout(false);
            this.grpNotaryInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabNetworkChat;
        private System.Windows.Forms.TabPage tabFileNotary;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMyPort;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox rtbChatLog;
        private System.Windows.Forms.GroupBox grpMessaging;
        private System.Windows.Forms.TextBox txtMessageInput;
        private System.Windows.Forms.Button btnEncryptSend;
        private System.Windows.Forms.Button btnSignFile;
        private System.Windows.Forms.Button btnVerifyFile;
        private System.Windows.Forms.GroupBox grpNotaryInfo;
        private System.Windows.Forms.Label lblNotaryStatus;
        private System.Windows.Forms.RichTextBox rtbNotaryLog;
    }
}
