using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ECC_Demo
{
    public partial class Form1 : Form
    {
        // ECDH Characters (Nullable to avoid warnings before generation)
        private ECDiffieHellmanCng? alice;
        private ECDiffieHellmanCng? bob;
        private byte[]? alicePublicKey;
        private byte[]? bobPublicKey;
        private byte[]? sharedSecret;

        // ECDSA Signer
        private ECDsaCng? notary;

        public Form1()
        {
            InitializeComponent();
        }

        // --- TAB 1: Secure Chat (ECDH) ---

        private void btnAliceGenerate_Click(object sender, EventArgs e)
        {
            // Initialize Alice with P-256 curve
            alice = new ECDiffieHellmanCng(256);
            alice.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            alice.HashAlgorithm = CngAlgorithm.Sha256;
            
            // Export public key to exchange (Using EccPublicBlob for CNG compatibility)
            // Note: ToByteArray is deprecated in newer .NET but works for CngKey.Import with blobs.
            // For cross-platform, ExportSubjectPublicKeyInfo is preferred, but this is a WinForms/CNG demo.
            #pragma warning disable SYSLIB0043
            alicePublicKey = alice.PublicKey.ToByteArray();
            #pragma warning restore SYSLIB0043
            
            txtAlicePublicKey.Text = Convert.ToBase64String(alicePublicKey);
            
            LogHandshake("Alice generated keys.");
        }

        private void btnBobGenerate_Click(object sender, EventArgs e)
        {
            bob = new ECDiffieHellmanCng(256);
            bob.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
            bob.HashAlgorithm = CngAlgorithm.Sha256;

            #pragma warning disable SYSLIB0043
            bobPublicKey = bob.PublicKey.ToByteArray();
            #pragma warning restore SYSLIB0043

            txtBobPublicKey.Text = Convert.ToBase64String(bobPublicKey);

            LogHandshake("Bob generated keys.");
        }

        private void btnHandshake_Click(object sender, EventArgs e)
        {
            if (alice == null || bob == null || alicePublicKey == null || bobPublicKey == null)
            {
                MessageBox.Show("Both parties must generate keys first.");
                return;
            }

            // Alice derives secret using Bob's public key
            byte[] aliceSecret = alice.DeriveKeyMaterial(CngKey.Import(bobPublicKey, CngKeyBlobFormat.EccPublicBlob));
            
            // Bob derives secret using Alice's public key
            byte[] bobSecret = bob.DeriveKeyMaterial(CngKey.Import(alicePublicKey, CngKeyBlobFormat.EccPublicBlob));

            // Verify they match
            bool match = true;
            if (aliceSecret.Length != bobSecret.Length) match = false;
            for(int i=0; i<aliceSecret.Length; i++) if(aliceSecret[i] != bobSecret[i]) match = false;

            if (match)
            {
                sharedSecret = aliceSecret;
                lblHandshakeStatus.Text = "Status: Shared Secret ESTABLISHED";
                lblHandshakeStatus.ForeColor = System.Drawing.Color.Green;
                LogHandshake("Handshake Successful. Shared Secret Derived.");
            }
            else
            {
                lblHandshakeStatus.Text = "Status: FAILED";
                lblHandshakeStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnEncryptSend_Click(object sender, EventArgs e)
        {
            if (sharedSecret == null)
            {
                MessageBox.Show("Perform handshake first to derive the shared key.");
                return;
            }

            string originalText = txtOriginalMessage.Text;
            byte[] plainBytes = Encoding.UTF8.GetBytes(originalText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = sharedSecret; 
                aes.GenerateIV();
                
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    
                    byte[] message = new byte[aes.IV.Length + cipherBytes.Length];
                    Array.Copy(aes.IV, 0, message, 0, aes.IV.Length);
                    Array.Copy(cipherBytes, 0, message, aes.IV.Length, cipherBytes.Length);

                    string hex = BitConverter.ToString(message).Replace("-", "");
                    txtEncryptedHex.Text = hex;

                    DecryptMessage(message); 
                }
            }
        }

        private void DecryptMessage(byte[] messageWithIv)
        {
            if (sharedSecret == null) return;
            try 
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = sharedSecret;
                    
                    int ivLength = aes.BlockSize / 8;
                    byte[] iv = new byte[ivLength];
                    byte[] cipher = new byte[messageWithIv.Length - ivLength];
                    
                    Array.Copy(messageWithIv, 0, iv, 0, iv.Length);
                    Array.Copy(messageWithIv, iv.Length, cipher, 0, cipher.Length);
                    
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        byte[] plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                        txtDecryptedMessage.Text = Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (Exception ex)
            {
                 txtDecryptedMessage.Text = "Decryption Error: " + ex.Message;
            }
        }

        private void LogHandshake(string msg)
        {
        }


        // --- TAB 2: Digital Notary (ECDSA) ---

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (notary == null)
            {
                notary = new ECDsaCng(256);
                notary.HashAlgorithm = CngAlgorithm.Sha256;
            }

            byte[] data = Encoding.UTF8.GetBytes(txtSignInput.Text);
            byte[] signature = notary.SignData(data); // ECDsaCng handles SHA256 internally

            txtSignature.Text = Convert.ToBase64String(signature);
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (notary == null || string.IsNullOrEmpty(txtSignature.Text))
            {
                MessageBox.Show("Sign a message first.");
                return;
            }

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txtSignInput.Text);
                byte[] signature = Convert.FromBase64String(txtSignature.Text);

                bool isValid = notary.VerifyData(data, signature);

                if (isValid)
                    MessageBox.Show("Signature is VALID. Content is authentic.", "Verification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Signature is INVALID! Content has been tampered with or signature is wrong.", "Verification", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verification error: " + ex.Message);
            }
        }

        private void btnTamper_Click(object sender, EventArgs e)
        {
            txtSignInput.Text += ".";
        }
    }
}
