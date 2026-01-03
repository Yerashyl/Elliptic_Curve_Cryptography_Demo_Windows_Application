using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECC_Demo
{
    public partial class Form1 : Form
    {
        // Networking
        private TcpListener? _server;
        private TcpClient? _client;
        private NetworkStream? _stream;
        private bool _isServer = false; // Unused

        // Cryptography - ECDH (Chat)
        private ECDiffieHellmanCng? _myEcdh;
        private byte[]? _sharedSecret;

        // Cryptography - ECDSA (Notary)
        // We create fresh keys for each signing operation or could persist them.
        // For this demo: Generate on Sign, Load on Verify.

        public Form1()
        {
            InitializeComponent();
        }

        // --- TAB 1: Network Chat (ECDHE + ECIES) ---

        // 1. Connection Logic
        private async void btnStartServer_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(txtMyPort.Text);
                _server = new TcpListener(IPAddress.Any, port);
                _server.Start();
                _isServer = true;
                Log($"Server started on port {port}. Waiting for peer...", Color.White);

                // Accept one client
                _client = await _server.AcceptTcpClientAsync();
                Log("Peer connected!", Color.Green);
                
                _stream = _client.GetStream();
                
                // Start Handshake immediately
                await PerformHandshakeAsync();

                // Start Listening Loop
                _ = ReceiveLoopAsync();
            }
            catch (Exception ex)
            {
                Log($"Server Error: {ex.Message}", Color.Red);
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                int port = int.Parse(txtTargetPort.Text);
                _client = new TcpClient();
                Log($"Connecting to localhost:{port}...", Color.White);
                await _client.ConnectAsync("127.0.0.1", port);
                Log("Connected to Peer!", Color.Green);

                _stream = _client.GetStream();

                // Start Handshake immediately
                await PerformHandshakeAsync();

                // Start Listening Loop
                _ = ReceiveLoopAsync();

            }
            catch (Exception ex)
            {
                Log($"Connection Error: {ex.Message}", Color.Red);
            }
        }

        // 2. ECDH Logic & Handshake
        private async Task PerformHandshakeAsync()
        {
            try
            {
                // Generate Ephemeral Keys
                _myEcdh = new ECDiffieHellmanCng(256);
                _myEcdh.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                _myEcdh.HashAlgorithm = CngAlgorithm.Sha256;
                Log("Generated Ephemeral ECDH Key Pair (P-256).", Color.Cyan);

                // Export Public Key
                #pragma warning disable SYSLIB0043
                byte[] myPublicKey = _myEcdh.PublicKey.ToByteArray();
                #pragma warning restore SYSLIB0043

                // Send Public Key Packet (Type 1)
                await SendPacketAsync(1, myPublicKey);
                Log("Sent Public Key to Peer.", Color.White);
            }
            catch (Exception ex)
            {
                Log($"Handshake Init Error: {ex.Message}", Color.Red);
            }
        }

        // 3. Packet Handling & Receive Loop
        private async Task ReceiveLoopAsync()
        {
            if (_stream == null) return;

            byte[] headerBuffer = new byte[5]; // [Type:1][Length:4]

            try
            {
                while (_client != null && _client.Connected)
                {
                    // Read Header
                    int bytesRead = await _stream.ReadAsync(headerBuffer, 0, 5);
                    if (bytesRead == 0) break; // Disconnected

                    byte type = headerBuffer[0];
                    int length = BitConverter.ToInt32(headerBuffer, 1);

                    // Read Payload
                    byte[] payload = new byte[length];
                    int totalRead = 0;
                    while (totalRead < length)
                    {
                        int read = await _stream.ReadAsync(payload, totalRead, length - totalRead);
                        if (read == 0) break;
                        totalRead += read;
                    }

                    // Process Packet
                    ProcessPacket(type, payload);
                }
            }
            catch (Exception ex)
            {
                Log($"Receive Error: {ex.Message}", Color.Red);
            }
            finally
            {
                Log("Disconnected.", Color.Orange);
            }
        }

        private void ProcessPacket(byte type, byte[] payload)
        {
            // Marshal Key Derivation / Decryption to UI thread for logging safety
            // (Or keep logic here and only Invoke the Log calls. We'll Invoke Log)

            if (type == 1) // Handshake (Peer Public Key)
            {
                try
                {
                    Log($"Received Peer Public Key ({payload.Length} bytes).", Color.White);
                    if (_myEcdh == null) return;

                    // Provide Import method proper hint
                    using (CngKey peerKey = CngKey.Import(payload, CngKeyBlobFormat.EccPublicBlob))
                    {
                        _sharedSecret = _myEcdh.DeriveKeyMaterial(peerKey);
                    }

                    Log("Derived Shared Secret successfully!", Color.Lime);
                    Log($"Secret Hash: {BitConverter.ToString(_sharedSecret).Substring(0, 10)}...", Color.Gray);
                }
                catch (Exception ex)
                {
                    Log($"Handshake Error: {ex.Message}", Color.Red);
                }
            }
            else if (type == 2) // Encrypted Message
            {
                try
                {
                    Log($"Received Encrypted Message ({payload.Length} bytes).", Color.White);
                    if (_sharedSecret == null)
                    {
                        Log("Error: No Shared Secret. Cannot Decrypt.", Color.Red);
                        return;
                    }

                    // Decrypt
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = _sharedSecret;
                        
                        // Extract IV (First 16 bytes for AES)
                        byte[] iv = new byte[aes.BlockSize / 8];
                        byte[] cipherText = new byte[payload.Length - iv.Length];

                        Array.Copy(payload, 0, iv, 0, iv.Length);
                        Array.Copy(payload, iv.Length, cipherText, 0, cipherText.Length);

                        aes.IV = iv;

                        using (var decryptor = aes.CreateDecryptor())
                        {
                            byte[] plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                            string message = Encoding.UTF8.GetString(plainBytes);
                            
                            Log($"Peer: {message}", Color.Yellow);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log($"Decryption Error: {ex.Message}", Color.Red);
                }
            }
        }

        private async Task SendPacketAsync(byte type, byte[] payload)
        {
            if (_stream == null) return;

            // Packet Structure: [Type:1][Length:4][Payload:N]
            byte[] lengthBytes = BitConverter.GetBytes(payload.Length);
            byte[] packet = new byte[1 + 4 + payload.Length];

            packet[0] = type;
            Array.Copy(lengthBytes, 0, packet, 1, 4);
            Array.Copy(payload, 0, packet, 5, payload.Length);

            await _stream.WriteAsync(packet, 0, packet.Length);
        }

        // 4. Messaging Logic
        private async void btnEncryptSend_Click(object sender, EventArgs e)
        {
            if (_stream == null || _sharedSecret == null)
            {
                Log("Not connected or Handshake incomplete.", Color.Red);
                return;
            }

            string text = txtMessageInput.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _sharedSecret;
                    aes.GenerateIV();

                    byte[] plainBytes = Encoding.UTF8.GetBytes(text);
                    
                    using (var encryptor = aes.CreateEncryptor())
                    {
                        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                        // Payload = [IV] + [Cipher]
                        byte[] payload = new byte[aes.IV.Length + cipherBytes.Length];
                        Array.Copy(aes.IV, 0, payload, 0, aes.IV.Length);
                        Array.Copy(cipherBytes, 0, payload, aes.IV.Length, cipherBytes.Length);

                        await SendPacketAsync(2, payload);
                        
                        Log($"Me: {text}", Color.Cyan); // Show own message locally (plaintext)
                        Log($"[Sent Encrypted ({payload.Length} bytes)]", Color.Gray);
                        txtMessageInput.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Encryption/Send Error: {ex.Message}", Color.Red);
            }
        }

        // --- Logging Helper ---
        private void Log(string message, Color color)
        {
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
            }
        }


        // --- TAB 2: File Notary (ECDSA) ---

        private void btnSignFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Title = "Select File to Sign" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes(ofd.FileName);
                        
                        // Generate new Signing Keys
                        using (ECDsaCng dsa = new ECDsaCng(256))
                        {
                            dsa.HashAlgorithm = CngAlgorithm.Sha256;
                            
                            // Sign
                            byte[] signature = dsa.SignData(data); // SHA256 implicitly

                            // Export Public Key
                            #pragma warning disable SYSLIB0043
                            byte[] publicKey = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
                            #pragma warning restore SYSLIB0043

                            // Save .sig and .pub
                            string sigPath = ofd.FileName + ".sig";
                            string pubPath = ofd.FileName + ".pub";
                            
                            File.WriteAllBytes(sigPath, signature);
                            File.WriteAllBytes(pubPath, publicKey);

                            lblNotaryStatus.Text = "Status: File Signed Successfully!";
                            lblNotaryStatus.ForeColor = Color.Green;
                            MessageBox.Show($"Created:\n{sigPath}\n{pubPath}", "Signing Complete");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Signing Error: {ex.Message}");
                    }
                }
            }
        }

        private void btnVerifyFile_Click(object sender, EventArgs e)
        {
            // We need 3 files: Original, .sig, .pub
            string? origFile = null, sigFile = null, keyFile = null;

            // Simple flow: user picks original, we assume .sig and .pub are same name, 
            // OR we ask for all 3. Prompt says "Opens OpenFileDialog three times".
            
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

            try
            {
                byte[] data = File.ReadAllBytes(origFile);
                byte[] signature = File.ReadAllBytes(sigFile);
                byte[] keyBytes = File.ReadAllBytes(keyFile);

                using (CngKey key = CngKey.Import(keyBytes, CngKeyBlobFormat.EccPublicBlob))
                using (ECDsaCng dsa = new ECDsaCng(key))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;
                    
                    bool valid = dsa.VerifyData(data, signature);

                    if (valid)
                    {
                        lblNotaryStatus.Text = "Status: VALID SIGNATURE";
                        lblNotaryStatus.ForeColor = Color.Green;
                        Log("[Notary] Signature Valid verified.", Color.Lime);
                        MessageBox.Show("Signature is VALID.", "Verification Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        lblNotaryStatus.Text = "Status: TAMPERED / INVALID";
                        lblNotaryStatus.ForeColor = Color.Red;
                        Log("[Notary] Signature INVALID.", Color.Red);
                        MessageBox.Show("Signature is INVALID.", "Verification Result", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Verification Error: {ex.Message}");
            }
        }
    }
}
