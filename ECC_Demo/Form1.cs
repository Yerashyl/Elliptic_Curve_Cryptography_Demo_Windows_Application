namespace ECC_Demo
{
    public partial class Form1 : Form
    {
        // Helper Services
        private readonly ECCMessagingService _messagingService;
        private readonly ECCNotaryService _notaryService;

        public Form1()
        {
            InitializeComponent();

            // Initialize services with callbacks for UI updates
            _messagingService = new ECCMessagingService(Log, OnChatMessageReceived);
            _notaryService = new ECCNotaryService(Log);
        }

        // ========================================================================
        // TAB 1: Network Chat (Delegated to ECCMessagingService)
        // ========================================================================

        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtMyPort.Text, out int port))
            {
                await _messagingService.StartServerAsync(port);
            }
            else
            {
                Log("[ERROR] Invalid Port Number", Color.Red);
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTargetPort.Text, out int port))
            {
                await _messagingService.ConnectToPeerAsync(port);
            }
            else
            {
                Log("[ERROR] Invalid Target Port Number", Color.Red);
            }
        }

        private async void btnEncryptSend_Click(object sender, EventArgs e)
        {
            string text = txtMessageInput.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            await _messagingService.SendEncryptedMessageAsync(text);

            // Clear input after sending (assuming success for UI flow)
            txtMessageInput.Clear();
        }

        // Callback invoked by the service when a decrypted message arrives
        private void OnChatMessageReceived(string message)
        {
            if (rtbChatLog.InvokeRequired)
            {
                rtbChatLog.Invoke(new Action(() => OnChatMessageReceived(message)));
            }
            else
            {
                Log($"[CHAT] Peer says: {message}", Color.Cyan);
            }
        }

        // ========================================================================
        // TAB 2: File Notary (Delegated to ECCNotaryService)
        // ========================================================================

        private void btnSignFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "Select File to Sign" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    bool success = _notaryService.SignFile(ofd.FileName);

                    if (success)
                    {
                        lblNotaryStatus.Text = "Status: File Signed Successfully!";
                        lblNotaryStatus.ForeColor = Color.Green;
                        MessageBox.Show($"Created signatures for {Path.GetFileName(ofd.FileName)}", "Signing Complete");
                    }
                    else
                    {
                        lblNotaryStatus.Text = "Status: Error Signing File";
                        lblNotaryStatus.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void btnVerifyFile_Click(object sender, EventArgs e)
        {
            string? origFile = null, sigFile = null, keyFile = null;

            // UI Logic for file selection remains in the Form
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "1. Select ORIGINAL File" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                origFile = ofd.FileName;
            }

            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "2. Select SIGNATURE (.sig) File" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                sigFile = ofd.FileName;
            }

            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "3. Select PUBLIC KEY (.pub) File" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                keyFile = ofd.FileName;
            }

            // Delegate verification logic to service
            bool isValid = _notaryService.VerifyFile(origFile, sigFile, keyFile);

            if (isValid)
            {
                lblNotaryStatus.Text = "Status: VALID SIGNATURE";
                lblNotaryStatus.ForeColor = Color.Green;
                MessageBox.Show("Signature is VALID.", "Verification Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblNotaryStatus.Text = "Status: TAMPERED / INVALID";
                lblNotaryStatus.ForeColor = Color.Red;
                MessageBox.Show("Signature is INVALID.", "Verification Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================================================
        // SHARED: Logging (Marshaling to UI Thread)
        // ========================================================================

        private void Log(string message, Color color)
        {
            if (rtbChatLog.IsDisposed) return;

            if (rtbChatLog.InvokeRequired)
            {
                rtbChatLog.Invoke(new Action(() => Log(message, color)));
            }
            else
            {
                rtbChatLog.SelectionStart = rtbChatLog.TextLength;
                rtbChatLog.SelectionLength = 0;
                rtbChatLog.SelectionColor = color;
                rtbChatLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                rtbChatLog.SelectionColor = rtbChatLog.ForeColor;
                rtbChatLog.ScrollToCaret();
            }
        }
    }
}