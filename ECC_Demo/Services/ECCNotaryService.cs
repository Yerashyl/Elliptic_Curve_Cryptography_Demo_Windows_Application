using System.Security.Cryptography;

namespace ECC_Demo
{
    /// <summary>
    /// Encapsulates logic for signing and verifying files using ECDSA (Elliptic Curve Digital Signature Algorithm).
    /// </summary>
    public class ECCNotaryService
    {
        private readonly Action<string, Color> _logger;

        /// <summary>
        /// Initializes a new instance of the ECCNotaryService.
        /// </summary>
        /// <param name="logger">Callback for logging status messages.</param>
        public ECCNotaryService(Action<string, Color> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generates a new key pair, hashes the file content, creates a signature, and saves the .sig and .pub files.
        /// </summary>
        /// <param name="filePath">The path to the file to be signed.</param>
        /// <returns>True if signing was successful, otherwise false.</returns>
        public bool SignFile(string filePath)
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                _logger($"[ECDSA] Loading file: {Path.GetFileName(filePath)}", Color.Cyan);

                byte[] data = File.ReadAllBytes(filePath);
                _logger($"        Size: {data.Length} bytes", Color.Gray);

                // Generate new Signing Keys
                _logger("[ECDSA] Generating new P-256 Signing Keypair...", Color.White);
                using (ECDsaCng dsa = new ECDsaCng(256))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;

                    // Sign Data
                    _logger("[ECDSA] Hashing data (SHA-256) and creating signature...", Color.White);
                    byte[] signature = dsa.SignData(data);

                    string sigBase64 = Convert.ToBase64String(signature);
                    _logger($"[ECDSA] Signature Generated ({signature.Length} bytes)!", Color.Lime);
                    _logger($"        Sig: {sigBase64.Substring(0, 50)}...", Color.Lime);

                    // Export Public Key
#pragma warning disable SYSLIB0043
                    byte[] publicKey = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
#pragma warning restore SYSLIB0043

                    // Define output paths
                    string sigPath = filePath + ".sig";
                    string pubPath = filePath + ".pub";

                    File.WriteAllBytes(sigPath, signature);
                    File.WriteAllBytes(pubPath, publicKey);

                    _logger($"[ECDSA] Saved signature to: {Path.GetFileName(sigPath)}", Color.White);
                    _logger($"[ECDSA] Saved public key to: {Path.GetFileName(pubPath)}", Color.White);
                    _logger("---------------------------------------------------------------", Color.Gray);

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Signing Error: {ex.Message}", Color.Red);
                return false;
            }
        }

        /// <summary>
        /// Verifies a file against a provided signature and public key.
        /// </summary>
        /// <param name="originalPath">Path to the original file.</param>
        /// <param name="signaturePath">Path to the .sig file.</param>
        /// <param name="publicKeyPath">Path to the .pub file.</param>
        /// <returns>True if valid, False if invalid or tampered.</returns>
        public bool VerifyFile(string originalPath, string signaturePath, string publicKeyPath)
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                _logger("[ECDSA] Starting Verification...", Color.Cyan);
                _logger($"        Original: {Path.GetFileName(originalPath)}", Color.Gray);
                _logger($"        Signature: {Path.GetFileName(signaturePath)}", Color.Gray);

                byte[] data = File.ReadAllBytes(originalPath);
                byte[] signature = File.ReadAllBytes(signaturePath);
                byte[] keyBytes = File.ReadAllBytes(publicKeyPath);

                _logger("[ECDSA] Importing Public Key...", Color.White);

                using (CngKey key = CngKey.Import(keyBytes, CngKeyBlobFormat.EccPublicBlob))
                using (ECDsaCng dsa = new ECDsaCng(key))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;

                    _logger("[ECDSA] Verifying data hash against signature...", Color.White);
                    bool valid = dsa.VerifyData(data, signature);

                    if (valid)
                    {
                        _logger("[ECDSA] RESULT: VALID SIGNATURE. File is Authentic.", Color.Lime);
                    }
                    else
                    {
                        _logger("[ECDSA] RESULT: INVALID. The file has been TAMPERED with!", Color.Red);
                    }

                    _logger("---------------------------------------------------------------", Color.Gray);
                    return valid;
                }
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Verification Error: {ex.Message}", Color.Red);
                // In case of error (e.g., bad key format), we treat it as verification failure for safety
                return false;
            }
        }
    }
}