using System.Security.Cryptography;

namespace ECC_Demo
{
    /// <summary>
    /// Service responsible for file signing and verification using ECDSA.
    /// Provides detailed logging of the Hashing -> Signing process.
    /// </summary>
    public class ECCNotaryService
    {
        private readonly Action<string, Color> _logger;

        public ECCNotaryService(Action<string, Color> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Calculates the SHA-256 hash of the file stream, signs the hash, and saves artifacts.
        /// </summary>
        public bool SignFile(string filePath)
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                _logger($"[ECDSA] START SIGNING PROCESS", Color.Yellow);
                _logger($"[FILE]  Target: {Path.GetFileName(filePath)}", Color.Cyan);

                // 1. COMPUTE HASH (Digest)
                // We do this explicitly to show the user that we sign the HASH, not the file directly.
                byte[] fileHash;
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    _logger($"[FILE]  Size: {fs.Length} bytes. Stream opened.", Color.Gray);
                    _logger("[HASH]  Calculating SHA-256 Digest of file...", Color.White);
                    fileHash = SHA256.HashData(fs);
                }

                string hashHex = BitConverter.ToString(fileHash).Replace("-", "");
                _logger($"[HASH]  SHA-256 Digest: {hashHex}", Color.Cyan);

                // 2. GENERATE KEYS
                _logger("[KEYS]  Generating new ECDSA Key Pair (Curve P-256)...", Color.White);
                using (ECDsaCng dsa = new ECDsaCng(256))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;

                    // Export private key parameters for DEMO visualization
                    try
                    {
                        var p = dsa.ExportParameters(true);
                        if (p.D != null)
                            _logger($"[KEYS]  Private (D): {BitConverter.ToString(p.D).Replace("-", "")}", Color.Gray);
                        if (p.Q.X != null)
                            _logger($"[KEYS]  Public (X):  {BitConverter.ToString(p.Q.X).Replace("-", "")}", Color.Gray);
                        if (p.Q.Y != null)
                            _logger($"[KEYS]  Public (Y):  {BitConverter.ToString(p.Q.Y).Replace("-", "")}", Color.Gray);
                    }
                    catch { /* Ignore if OS protects key */ }

                    // 3. CREATE SIGNATURE
                    _logger("[SIGN]  Encrypting Hash with Private Key...", Color.White);

                    // We sign the PRE-CALCULATED HASH.
                    byte[] signature = dsa.SignHash(fileHash);

                    string sigBase64 = Convert.ToBase64String(signature);
                    _logger($"[SIGN]  Signature Generated ({signature.Length} bytes)!", Color.Lime);
                    _logger($"[SIGN]  Value: {sigBase64.Substring(0, 60)}...", Color.Lime);

                    // 4. SAVE ARTIFACTS
#pragma warning disable SYSLIB0043
                    byte[] publicKey = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
#pragma warning restore SYSLIB0043

                    File.WriteAllBytes(filePath + ".sig", signature);
                    File.WriteAllBytes(filePath + ".pub", publicKey);

                    _logger($"[IO]    Saved: {Path.GetFileName(filePath)}.sig", Color.White);
                    _logger($"[IO]    Saved: {Path.GetFileName(filePath)}.pub", Color.White);
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
        /// Verifies a file by re-calculating its hash and validating the signature with the public key.
        /// </summary>
        public bool VerifyFile(string originalPath, string signaturePath, string publicKeyPath)
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                _logger("[ECDSA] START VERIFICATION PROCESS", Color.Yellow);
                _logger($"[FILE]  Original: {Path.GetFileName(originalPath)}", Color.Cyan);

                // 1. READ ARTIFACTS
                byte[] signature = File.ReadAllBytes(signaturePath);
                byte[] keyBytes = File.ReadAllBytes(publicKeyPath);
                _logger($"[IO]    Loaded Signature ({signature.Length} bytes) and Public Key ({keyBytes.Length} bytes).", Color.Gray);

                // 2. RE-CALCULATE HASH
                // Verification requires hashing the file again to ensure it hasn't changed.
                byte[] fileHash;
                using (FileStream fs = new FileStream(originalPath, FileMode.Open, FileAccess.Read))
                {
                    _logger("[HASH]  Re-calculating SHA-256 Digest of original file...", Color.White);
                    fileHash = SHA256.HashData(fs);
                }
                string hashHex = BitConverter.ToString(fileHash).Replace("-", "");
                _logger($"[HASH]  Current Digest: {hashHex}", Color.Cyan);

                // 3. VERIFY
                _logger("[VERIFY] Importing Public Key and checking Signature...", Color.White);

                using (CngKey key = CngKey.Import(keyBytes, CngKeyBlobFormat.EccPublicBlob))
                using (ECDsaCng dsa = new ECDsaCng(key))
                {
                    dsa.HashAlgorithm = CngAlgorithm.Sha256;

                    // Verify using the HASH
                    bool valid = dsa.VerifyHash(fileHash, signature);

                    if (valid)
                    {
                        _logger("[RESULT] VALID SIGNATURE. The file is AUTHENTIC.", Color.Lime);
                    }
                    else
                    {
                        _logger("[RESULT] INVALID. The file content does not match the signature.", Color.Red);
                    }

                    _logger("---------------------------------------------------------------", Color.Gray);
                    return valid;
                }
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Verification Error: {ex.Message}", Color.Red);
                return false;
            }
        }
    }
}