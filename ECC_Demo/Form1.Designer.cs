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
            this.tabSecureChat = new System.Windows.Forms.TabPage();
            this.grpMessaging = new System.Windows.Forms.GroupBox();
            this.txtDecryptedMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEncryptedHex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEncryptSend = new System.Windows.Forms.Button();
            this.txtOriginalMessage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grpHandshake = new System.Windows.Forms.GroupBox();
            this.lblHandshakeStatus = new System.Windows.Forms.Label();
            this.btnHandshake = new System.Windows.Forms.Button();
            this.grpBob = new System.Windows.Forms.GroupBox();
            this.txtBobPublicKey = new System.Windows.Forms.TextBox();
            this.btnBobGenerate = new System.Windows.Forms.Button();
            this.grpAlice = new System.Windows.Forms.GroupBox();
            this.txtAlicePublicKey = new System.Windows.Forms.TextBox();
            this.btnAliceGenerate = new System.Windows.Forms.Button();
            this.tabDigitalNotary = new System.Windows.Forms.TabPage();
            this.btnTamper = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.txtSignature = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSign = new System.Windows.Forms.Button();
            this.txtSignInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabSecureChat.SuspendLayout();
            this.grpMessaging.SuspendLayout();
            this.grpHandshake.SuspendLayout();
            this.grpBob.SuspendLayout();
            this.grpAlice.SuspendLayout();
            this.tabDigitalNotary.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSecureChat);
            this.tabControl1.Controls.Add(this.tabDigitalNotary);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSecureChat
            // 
            this.tabSecureChat.Controls.Add(this.grpMessaging);
            this.tabSecureChat.Controls.Add(this.grpHandshake);
            this.tabSecureChat.Controls.Add(this.grpBob);
            this.tabSecureChat.Controls.Add(this.grpAlice);
            this.tabSecureChat.Location = new System.Drawing.Point(4, 24);
            this.tabSecureChat.Name = "tabSecureChat";
            this.tabSecureChat.Padding = new System.Windows.Forms.Padding(3);
            this.tabSecureChat.Size = new System.Drawing.Size(792, 572);
            this.tabSecureChat.TabIndex = 0;
            this.tabSecureChat.Text = "Secure Chat (ECDH)";
            this.tabSecureChat.UseVisualStyleBackColor = true;
            // 
            // grpMessaging
            // 
            this.grpMessaging.Controls.Add(this.txtDecryptedMessage);
            this.grpMessaging.Controls.Add(this.label4);
            this.grpMessaging.Controls.Add(this.txtEncryptedHex);
            this.grpMessaging.Controls.Add(this.label3);
            this.grpMessaging.Controls.Add(this.btnEncryptSend);
            this.grpMessaging.Controls.Add(this.txtOriginalMessage);
            this.grpMessaging.Controls.Add(this.label2);
            this.grpMessaging.Location = new System.Drawing.Point(8, 260);
            this.grpMessaging.Name = "grpMessaging";
            this.grpMessaging.Size = new System.Drawing.Size(776, 300);
            this.grpMessaging.TabIndex = 3;
            this.grpMessaging.TabStop = false;
            this.grpMessaging.Text = "Messaging (Encrypted/Decrypted with Shared Secret)";
            // 
            // txtDecryptedMessage
            // 
            this.txtDecryptedMessage.Location = new System.Drawing.Point(450, 100);
            this.txtDecryptedMessage.Multiline = true;
            this.txtDecryptedMessage.Name = "txtDecryptedMessage";
            this.txtDecryptedMessage.ReadOnly = true;
            this.txtDecryptedMessage.Size = new System.Drawing.Size(300, 150);
            this.txtDecryptedMessage.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(450, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Decrypted (Bob Read Only)";
            // 
            // txtEncryptedHex
            // 
            this.txtEncryptedHex.Location = new System.Drawing.Point(20, 100);
            this.txtEncryptedHex.Multiline = true;
            this.txtEncryptedHex.Name = "txtEncryptedHex";
            this.txtEncryptedHex.ReadOnly = true;
            this.txtEncryptedHex.Size = new System.Drawing.Size(400, 150);
            this.txtEncryptedHex.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Encrypted (Hex/Base64)";
            // 
            // btnEncryptSend
            // 
            this.btnEncryptSend.Location = new System.Drawing.Point(600, 40);
            this.btnEncryptSend.Name = "btnEncryptSend";
            this.btnEncryptSend.Size = new System.Drawing.Size(150, 23);
            this.btnEncryptSend.TabIndex = 2;
            this.btnEncryptSend.Text = "Encrypt && Send";
            this.btnEncryptSend.UseVisualStyleBackColor = true;
            this.btnEncryptSend.Click += new System.EventHandler(this.btnEncryptSend_Click);
            // 
            // txtOriginalMessage
            // 
            this.txtOriginalMessage.Location = new System.Drawing.Point(120, 40);
            this.txtOriginalMessage.Name = "txtOriginalMessage";
            this.txtOriginalMessage.Size = new System.Drawing.Size(470, 23);
            this.txtOriginalMessage.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Original Message:";
            // 
            // grpHandshake
            // 
            this.grpHandshake.Controls.Add(this.lblHandshakeStatus);
            this.grpHandshake.Controls.Add(this.btnHandshake);
            this.grpHandshake.Location = new System.Drawing.Point(8, 160);
            this.grpHandshake.Name = "grpHandshake";
            this.grpHandshake.Size = new System.Drawing.Size(776, 80);
            this.grpHandshake.TabIndex = 2;
            this.grpHandshake.TabStop = false;
            this.grpHandshake.Text = "Handshake";
            // 
            // lblHandshakeStatus
            // 
            this.lblHandshakeStatus.AutoSize = true;
            this.lblHandshakeStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHandshakeStatus.Location = new System.Drawing.Point(200, 34);
            this.lblHandshakeStatus.Name = "lblHandshakeStatus";
            this.lblHandshakeStatus.Size = new System.Drawing.Size(82, 15);
            this.lblHandshakeStatus.TabIndex = 1;
            this.lblHandshakeStatus.Text = "Status: Idle";
            // 
            // btnHandshake
            // 
            this.btnHandshake.Location = new System.Drawing.Point(20, 30);
            this.btnHandshake.Name = "btnHandshake";
            this.btnHandshake.Size = new System.Drawing.Size(150, 23);
            this.btnHandshake.TabIndex = 0;
            this.btnHandshake.Text = "Perform Handshake";
            this.btnHandshake.UseVisualStyleBackColor = true;
            this.btnHandshake.Click += new System.EventHandler(this.btnHandshake_Click);
            // 
            // grpBob
            // 
            this.grpBob.Controls.Add(this.txtBobPublicKey);
            this.grpBob.Controls.Add(this.btnBobGenerate);
            this.grpBob.Location = new System.Drawing.Point(400, 10);
            this.grpBob.Name = "grpBob";
            this.grpBob.Size = new System.Drawing.Size(384, 140);
            this.grpBob.TabIndex = 1;
            this.grpBob.TabStop = false;
            this.grpBob.Text = "Bob";
            // 
            // txtBobPublicKey
            // 
            this.txtBobPublicKey.Location = new System.Drawing.Point(20, 60);
            this.txtBobPublicKey.Multiline = true;
            this.txtBobPublicKey.Name = "txtBobPublicKey";
            this.txtBobPublicKey.ReadOnly = true;
            this.txtBobPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBobPublicKey.Size = new System.Drawing.Size(340, 70);
            this.txtBobPublicKey.TabIndex = 1;
            // 
            // btnBobGenerate
            // 
            this.btnBobGenerate.Location = new System.Drawing.Point(20, 30);
            this.btnBobGenerate.Name = "btnBobGenerate";
            this.btnBobGenerate.Size = new System.Drawing.Size(150, 23);
            this.btnBobGenerate.TabIndex = 0;
            this.btnBobGenerate.Text = "Generate Keys";
            this.btnBobGenerate.UseVisualStyleBackColor = true;
            this.btnBobGenerate.Click += new System.EventHandler(this.btnBobGenerate_Click);
            // 
            // grpAlice
            // 
            this.grpAlice.Controls.Add(this.txtAlicePublicKey);
            this.grpAlice.Controls.Add(this.btnAliceGenerate);
            this.grpAlice.Location = new System.Drawing.Point(8, 10);
            this.grpAlice.Name = "grpAlice";
            this.grpAlice.Size = new System.Drawing.Size(384, 140);
            this.grpAlice.TabIndex = 0;
            this.grpAlice.TabStop = false;
            this.grpAlice.Text = "Alice";
            // 
            // txtAlicePublicKey
            // 
            this.txtAlicePublicKey.Location = new System.Drawing.Point(20, 60);
            this.txtAlicePublicKey.Multiline = true;
            this.txtAlicePublicKey.Name = "txtAlicePublicKey";
            this.txtAlicePublicKey.ReadOnly = true;
            this.txtAlicePublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAlicePublicKey.Size = new System.Drawing.Size(340, 70);
            this.txtAlicePublicKey.TabIndex = 1;
            // 
            // btnAliceGenerate
            // 
            this.btnAliceGenerate.Location = new System.Drawing.Point(20, 30);
            this.btnAliceGenerate.Name = "btnAliceGenerate";
            this.btnAliceGenerate.Size = new System.Drawing.Size(150, 23);
            this.btnAliceGenerate.TabIndex = 0;
            this.btnAliceGenerate.Text = "Generate Keys";
            this.btnAliceGenerate.UseVisualStyleBackColor = true;
            this.btnAliceGenerate.Click += new System.EventHandler(this.btnAliceGenerate_Click);
            // 
            // tabDigitalNotary
            // 
            this.tabDigitalNotary.Controls.Add(this.btnTamper);
            this.tabDigitalNotary.Controls.Add(this.btnVerify);
            this.tabDigitalNotary.Controls.Add(this.txtSignature);
            this.tabDigitalNotary.Controls.Add(this.label6);
            this.tabDigitalNotary.Controls.Add(this.btnSign);
            this.tabDigitalNotary.Controls.Add(this.txtSignInput);
            this.tabDigitalNotary.Controls.Add(this.label5);
            this.tabDigitalNotary.Location = new System.Drawing.Point(4, 24);
            this.tabDigitalNotary.Name = "tabDigitalNotary";
            this.tabDigitalNotary.Padding = new System.Windows.Forms.Padding(3);
            this.tabDigitalNotary.Size = new System.Drawing.Size(792, 572);
            this.tabDigitalNotary.TabIndex = 1;
            this.tabDigitalNotary.Text = "Digital Notary (ECDSA)";
            this.tabDigitalNotary.UseVisualStyleBackColor = true;
            // 
            // btnTamper
            // 
            this.btnTamper.ForeColor = System.Drawing.Color.Red;
            this.btnTamper.Location = new System.Drawing.Point(600, 20);
            this.btnTamper.Name = "btnTamper";
            this.btnTamper.Size = new System.Drawing.Size(150, 23);
            this.btnTamper.TabIndex = 6;
            this.btnTamper.Text = "Tamper (Invalidate)";
            this.btnTamper.UseVisualStyleBackColor = true;
            this.btnTamper.Click += new System.EventHandler(this.btnTamper_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(200, 250);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(150, 23);
            this.btnVerify.TabIndex = 5;
            this.btnVerify.Text = "Verify Signature";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // txtSignature
            // 
            this.txtSignature.Location = new System.Drawing.Point(20, 280);
            this.txtSignature.Multiline = true;
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.ReadOnly = true;
            this.txtSignature.Size = new System.Drawing.Size(730, 100);
            this.txtSignature.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 260);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "Signature (Base64)";
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(20, 210);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(150, 23);
            this.btnSign.TabIndex = 2;
            this.btnSign.Text = "Sign Message";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // txtSignInput
            // 
            this.txtSignInput.Location = new System.Drawing.Point(20, 50);
            this.txtSignInput.Multiline = true;
            this.txtSignInput.Name = "txtSignInput";
            this.txtSignInput.Size = new System.Drawing.Size(730, 150);
            this.txtSignInput.TabIndex = 1;
            this.txtSignInput.Text = "This is a document that will be signed using ECDSA.\r\nAny changes to this text will invalidate the digital signature.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Message Input:";
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
            this.tabSecureChat.ResumeLayout(false);
            this.grpMessaging.ResumeLayout(false);
            this.grpMessaging.PerformLayout();
            this.grpHandshake.ResumeLayout(false);
            this.grpHandshake.PerformLayout();
            this.grpBob.ResumeLayout(false);
            this.grpBob.PerformLayout();
            this.grpAlice.ResumeLayout(false);
            this.grpAlice.PerformLayout();
            this.tabDigitalNotary.ResumeLayout(false);
            this.tabDigitalNotary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSecureChat;
        private System.Windows.Forms.TabPage tabDigitalNotary;
        private System.Windows.Forms.GroupBox grpAlice;
        private System.Windows.Forms.GroupBox grpBob;
        private System.Windows.Forms.Button btnAliceGenerate;
        private System.Windows.Forms.Button btnBobGenerate;
        private System.Windows.Forms.TextBox txtAlicePublicKey;
        private System.Windows.Forms.TextBox txtBobPublicKey;
        private System.Windows.Forms.GroupBox grpHandshake;
        private System.Windows.Forms.Button btnHandshake;
        private System.Windows.Forms.Label lblHandshakeStatus;
        private System.Windows.Forms.GroupBox grpMessaging;
        private System.Windows.Forms.TextBox txtOriginalMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnEncryptSend;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEncryptedHex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDecryptedMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSignInput;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSignature;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnTamper;
    }
}
