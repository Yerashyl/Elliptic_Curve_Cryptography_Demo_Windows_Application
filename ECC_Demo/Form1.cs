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
            _notaryService = new ECCNotaryService(LogNotary);
        }

        // ========================================================================
        // TAB 1: Network Chat (Delegated to ECCMessagingService)
        // ========================================================================

        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtMyPort.Text, out int port))
                await _messagingService.StartServerAsync(port);
            else
                Log("[ERROR] Invalid Port Number", Color.Red);
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTargetPort.Text, out int port))
                await _messagingService.ConnectToPeerAsync(port);
            else
                Log("[ERROR] Invalid Target Port Number", Color.Red);
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
                rtbChatLog.Invoke(new Action(() => OnChatMessageReceived(message)));
            else
                Log($"[PEER] {message}", Color.Cyan);
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
                        lblNotaryStatus.Text = "Status: Signed Successfully";
                        lblNotaryStatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblNotaryStatus.Text = "Status: Signing Failed";
                        lblNotaryStatus.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void btnVerifyFile_Click(object sender, EventArgs e)
        {
            string? origFile = null, sigFile = null, keyFile = null;

            // 1. Select Content
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "1. Select ORIGINAL File", Filter = "All files (*.*)|*.*" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                origFile = ofd.FileName;
            }

            // 2. Select Signature (Filtered)
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "2. Select SIGNATURE (.sig)", Filter = "Signature (*.sig)|*.sig" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                sigFile = ofd.FileName;
            }

            // 3. Select Key (Filtered)
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "3. Select PUBLIC KEY (.pub)", Filter = "Public Key (*.pub)|*.pub" })
            {
                if (ofd.ShowDialog() != DialogResult.OK) return;
                keyFile = ofd.FileName;
            }

            bool isValid = _notaryService.VerifyFile(origFile, sigFile, keyFile);

            if (isValid)
            {
                lblNotaryStatus.Text = "Status: VALID SIGNATURE";
                lblNotaryStatus.ForeColor = Color.Green;
                MessageBox.Show("Signature is VALID.\nFile digest matches the signature.", "Verification Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblNotaryStatus.Text = "Status: INVALID / TAMPERED";
                lblNotaryStatus.ForeColor = Color.Red;
                MessageBox.Show("Signature is INVALID.\nFile has been modified or keys do not match.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========================================================================
        // SHARED: Logging
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
                rtbChatLog.ScrollToCaret(); // Ensure auto-scroll
            }
        }

        private void LogNotary(string message, Color color)
        {
            if (rtbNotaryLog.IsDisposed) return;
            if (rtbNotaryLog.InvokeRequired)
            {
                rtbNotaryLog.Invoke(new Action(() => LogNotary(message, color)));
            }
            else
            {
                rtbNotaryLog.SelectionStart = rtbNotaryLog.TextLength;
                rtbNotaryLog.SelectionLength = 0;
                rtbNotaryLog.SelectionColor = color;
                rtbNotaryLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
                rtbNotaryLog.ScrollToCaret(); // Ensure auto-scroll
            }
        }
    }
}