using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace ECC_Demo
{
    /// <summary>
    /// Encapsulates logic for Peer-to-Peer messaging using ECDHE for key exchange
    /// and AES (ECIES-like) for message encryption.
    /// </summary>
    public class ECCMessagingService
    {
        // Networking objects
        private TcpListener? _server;
        private TcpClient? _client;
        private NetworkStream? _stream;

        // Cryptography objects
        private ECDiffieHellmanCng? _ecdhKey;
        private byte[]? _sharedSecret;

        // Delegate for logging back to the UI
        private readonly Action<string, Color> _logger;
        // Delegate for notifying the UI when a chat message is received
        private readonly Action<string> _onMessageReceived;

        /// <summary>
        /// Initializes a new instance of the ECCMessagingService.
        /// </summary>
        /// <param name="logger">Callback for logging status messages.</param>
        /// <param name="onMessageReceived">Callback for handling received decrypted text.</param>
        public ECCMessagingService(Action<string, Color> logger, Action<string> onMessageReceived)
        {
            _logger = logger;
            _onMessageReceived = onMessageReceived;
        }

        /// <summary>
        /// Starts a TCP listener on the specified port and waits for a peer connection.
        /// </summary>
        /// <param name="port">The local port to listen on.</param>
        public async Task StartServerAsync(int port)
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, port);
                _server.Start();
                _logger($"[NET] Server started on port {port}.", Color.Cyan);
                _logger("[NET] Waiting for incoming connection...", Color.White);

                _client = await _server.AcceptTcpClientAsync();
                _logger("[NET] Peer connected successfully!", Color.Lime);

                _stream = _client.GetStream();

                // Initiate Handshake immediately upon connection
                await PerformHandshakeAsync();
                _ = ReceiveLoopAsync();
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Server Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Connects to a remote peer at the specified loopback port.
        /// </summary>
        /// <param name="port">The target port to connect to.</param>
        public async Task ConnectToPeerAsync(int port)
        {
            try
            {
                _client = new TcpClient();
                _logger($"[NET] Attempting connection to 127.0.0.1:{port}...", Color.Cyan);

                await _client.ConnectAsync("127.0.0.1", port);
                _logger("[NET] Connection established!", Color.Lime);

                _stream = _client.GetStream();

                // Initiate Handshake immediately upon connection
                await PerformHandshakeAsync();
                _ = ReceiveLoopAsync();
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Connection Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Encrypts and sends a text message using the derived shared secret.
        /// </summary>
        /// <param name="message">The plain text message to send.</param>
        public async Task SendEncryptedMessageAsync(string message)
        {
            if (_stream == null || _sharedSecret == null)
            {
                _logger("[ERROR] Not connected or Handshake incomplete.", Color.Red);
                return;
            }

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = _sharedSecret;
                    aes.GenerateIV();

                    byte[] plainBytes = Encoding.UTF8.GetBytes(message);

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                        // Payload structure: [IV (16 bytes)] + [Ciphertext]
                        byte[] payload = new byte[aes.IV.Length + cipherBytes.Length];
                        Array.Copy(aes.IV, 0, payload, 0, aes.IV.Length);
                        Array.Copy(cipherBytes, 0, payload, aes.IV.Length, cipherBytes.Length);

                        await SendPacketAsync(2, payload);

                        _logger($"[ME] {message}", Color.White);
                        _logger($"     [Encrypted] IV: {BitConverter.ToString(aes.IV).Replace("-", "")}", Color.Gray);
                        _logger($"     [Encrypted] Cipher: {BitConverter.ToString(cipherBytes).Replace("-", "")}", Color.Gray);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Encryption/Send Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Generates ephemeral ECDH keys and sends the public key to the peer.
        /// </summary>
        private async Task PerformHandshakeAsync()
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                _logger("[ECDHE] Starting Ephemeral Key Exchange...", Color.Yellow);

                // Generate Ephemeral Keys
                _ecdhKey = new ECDiffieHellmanCng(256);
                _ecdhKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
                _ecdhKey.HashAlgorithm = CngAlgorithm.Sha256;
                _logger("[ECDHE] Generated local Key Pair (Curve P-256).", Color.White);

#pragma warning disable SYSLIB0043
                byte[] myPublicKey = _ecdhKey.PublicKey.ToByteArray();
#pragma warning restore SYSLIB0043

                string hexKey = BitConverter.ToString(myPublicKey).Replace("-", "");
                _logger($"[ECDHE] My Public Key ({myPublicKey.Length} bytes):", Color.Gray);
                _logger($"        {hexKey.Substring(0, 32)}...", Color.Gray);

                // Send Public Key Packet (Type 1)
                await SendPacketAsync(1, myPublicKey);
                _logger("[NET] Sent Public Key packet to peer.", Color.White);
                _logger("---------------------------------------------------------------", Color.Gray);
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Handshake Init Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Continuously listens for incoming packets from the stream.
        /// </summary>
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

                    ProcessPacket(type, payload);
                }
            }
            catch (Exception ex)
            {
                _logger($"[NET] Receive Loop Error: {ex.Message}", Color.Red);
            }
            finally
            {
                _logger("[NET] Disconnected from peer.", Color.Orange);
            }
        }

        /// <summary>
        /// Processes a received packet based on its type.
        /// </summary>
        /// <param name="type">The packet type identifier (1 for Key, 2 for Message).</param>
        /// <param name="payload">The binary content of the packet.</param>
        private void ProcessPacket(byte type, byte[] payload)
        {
            if (type == 1) // Handshake (Peer Public Key)
            {
                HandlePeerPublicKey(payload);
            }
            else if (type == 2) // Encrypted Message
            {
                HandleEncryptedMessage(payload);
            }
        }

        /// <summary>
        /// Derives the shared secret from the peer's public key.
        /// </summary>
        /// <param name="publicKeyBytes">The peer's public key.</param>
        private void HandlePeerPublicKey(byte[] publicKeyBytes)
        {
            try
            {
                _logger("---------------------------------------------------------------", Color.Gray);
                string hexPeer = BitConverter.ToString(publicKeyBytes).Replace("-", "");
                _logger($"[NET] Received Peer Public Key ({publicKeyBytes.Length} bytes):", Color.Yellow);
                _logger($"      {hexPeer.Substring(0, 32)}...", Color.Gray);

                if (_ecdhKey == null) return;

                using (CngKey peerKey = CngKey.Import(publicKeyBytes, CngKeyBlobFormat.EccPublicBlob))
                {
                    _logger("[ECDH] Computing Shared Secret (ECDH)...", Color.White);
                    _sharedSecret = _ecdhKey.DeriveKeyMaterial(peerKey);
                }

                string hexSecret = BitConverter.ToString(_sharedSecret).Replace("-", "");
                _logger("[ECDH] Shared Secret Derived Successfully!", Color.Lime);
                _logger($"       Secret (AES Key): {hexSecret}", Color.Lime);
                _logger("---------------------------------------------------------------", Color.Gray);
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Handshake Processing Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Decrypts the incoming payload using the shared secret.
        /// </summary>
        /// <param name="payload">The encrypted payload (IV + Ciphertext).</param>
        private void HandleEncryptedMessage(byte[] payload)
        {
            try
            {
                _logger("[NET] Received Encrypted Payload:", Color.Yellow);
                _logger($"      Bytes: {BitConverter.ToString(payload).Replace("-", "")}", Color.Gray);

                if (_sharedSecret == null)
                {
                    _logger("[ERROR] No Shared Secret established. Cannot decrypt.", Color.Red);
                    return;
                }

                using (Aes aes = Aes.Create())
                {
                    aes.Key = _sharedSecret;

                    // Extract IV
                    byte[] iv = new byte[aes.BlockSize / 8];
                    byte[] cipherText = new byte[payload.Length - iv.Length];

                    Array.Copy(payload, 0, iv, 0, iv.Length);
                    Array.Copy(payload, iv.Length, cipherText, 0, cipherText.Length);

                    aes.IV = iv;

                    _logger($"[ECIES] Decrypting with Shared Secret...", Color.White);
                    _logger($"        IV: {BitConverter.ToString(iv).Replace("-", "")}", Color.Gray);

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        byte[] plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                        string message = Encoding.UTF8.GetString(plainBytes);

                        // Notify UI
                        _onMessageReceived(message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger($"[ERROR] Decryption Error: {ex.Message}", Color.Red);
            }
        }

        /// <summary>
        /// Helper method to serialize and write packets to the network stream.
        /// Packet Format: [Type (1 byte)] [Length (4 bytes)] [Payload (N bytes)]
        /// </summary>
        /// <param name="type">The type of packet.</param>
        /// <param name="payload">The data to send.</param>
        private async Task SendPacketAsync(byte type, byte[] payload)
        {
            if (_stream == null) return;

            byte[] lengthBytes = BitConverter.GetBytes(payload.Length);
            byte[] packet = new byte[1 + 4 + payload.Length];

            packet[0] = type;
            Array.Copy(lengthBytes, 0, packet, 1, 4);
            Array.Copy(payload, 0, packet, 5, payload.Length);

            await _stream.WriteAsync(packet, 0, packet.Length);
        }
    }
}