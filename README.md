# ECC Crypto-Dashboard: Secure Messaging & Digital Notary

A comprehensive C# Windows Forms application demonstrating the practical implementation of **Elliptic Curve Cryptography (ECC)**.

This tool provides a hands-on visualization of how modern cryptography handles secure key exchange, encryption, and digital signatures.

---

## 🚀 Features

### 1. Secure Peer-to-Peer Chat (ECDHE + AES)
* **Perfect Forward Secrecy:** Uses **ECDHE** (Elliptic Curve Diffie-Hellman Ephemeral) to generate unique session keys for every connection;
* **Hybrid Encryption:** Derives a shared secret to encrypt messages using **AES** (Advanced Encryption Standard);
* **Transparent Handshake:** Visualizes the entire key exchange process, exposing public keys and shared secret derivation in real-time logs.

### 2. Digital File Notary (ECDSA)
* **File Integrity:** Signs files of any size (using memory-efficient streaming) by calculating their **SHA-256** digest;
* **Authentication:** Uses **ECDSA** (Elliptic Curve Digital Signature Algorithm) with the **P-256** curve to create tamper-proof signatures;
* **Verification:** Verifies files against `.sig` (Signature) and `.pub` (Public Key) artifacts to ensure authenticity and detect tampering.

---

## 🛠️ Technical Overview

### 🔐 Messaging Architecture (The "Chat" Tab)
The application implements a custom protocol to simulate a secure channel:
1.  **Network Setup:** Establishes a TCP connection between two peers (Server/Client);
2.  **ECDHE Handshake:**
    * Both parties generate an ephemeral **P-256** key pair;
    * They exchange **Public Keys** over the network;
    * They independently compute the same **Shared Secret** using their Private Key and the peer's Public Key;
3.  **AES Encryption:**
    * The Shared Secret is used as the **AES Key**;
    * Every message is encrypted with a unique **IV** (Initialization Vector);
    * Payload = `[IV] + [Ciphertext]`.

**Log Output Example:**

```text
[ECDHE] Generated local Key Pair (Curve P-256).
[KEY] My Public (X):  8D6EC35CAF3D05D1...
[NET] Received Peer Public Key (72 bytes).
[ECDH] Formula: SharedSecret = ECDH(MyPrivate, PeerPublic)
[ECDH] Shared Secret Derived Successfully!
[AES] Key: C15C017C1FEFDAC9...
```

### ✍️ Notary Architecture (The "Notary" Tab)

The application demonstrates that digital signatures sign the *hash* of the data, not the data itself:

1. **Hashing:** The file is read via a `FileStream` and hashed using **SHA-256**;
2. **Signing:** The hash is signed using a newly generated **ECDSA P-256** private key;
3. **Artifacts:** The app exports:
    * `filename.ext.sig`: The raw signature;
    * `filename.ext.pub`: The public key needed for verification.

**Log Output Example:**

```text
[ECDSA] START SIGNING PROCESS
[FILE]  Target: TestABC.txt
[HASH]  Calculating SHA-256 Digest of file...
[HASH]  SHA-256 Digest: 262E7D83CE33...
[SIGN]  Encrypting Hash with Private Key...
[IO]    Saved: TestABC.txt.sig
```

---

## 💻 Getting Started

### Prerequisites

* .NET 9.0;
* Visual Studio 2022 (or later);
* Windows OS (WinForms support).

### Installation

1. Clone the repository:
```bash
git clone https://github.com/BogdanCTU/Elliptic_Curve_Cryptography_Demo_Windows_Application
```
2. Open the solution in **Visual Studio**;
3. Build and Run (`F5`).

---

## 📖 Usage Guide

### 1. Testing Secure Chat

To simulate a chat, you can run two instances of the application on the same machine.

**Instance A (Server):**

1. Go to the **Network Chat** tab;
2. Set **My Port** to `5000`;
3. Click **Start Server**.

**Instance B (Client):**

1. Go to the **Network Chat** tab;
2. Set **Target Port** to `5000`;
3. Click **Connect to Peer**.

*Watch the "Chat Log" to see the ECDHE handshake occur immediately.*

### 2. Signing a File

1. Go to the **File Notary** tab;
2. Click **Sign File (Create .sig & .pub)**;
3. Select any file (e.g., `document.pdf`);
4. The tool will generate `.sig` and `.pub` files in the same directory.

### 3. Verifying a File

1. Click **Verify File Signature**;
2. **Step 1:** Select the **Original File** (e.g., `document.pdf`);
3. **Step 2:** Select the **Signature File** (`document.pdf.sig`);
4. **Step 3:** Select the **Public Key File** (`document.pdf.pub`);
5. The system will report if the file is **VALID** (Authentic) or **INVALID** (Tampered).
---

## 📸 Sample Logs

### Successful Encryption (Chat)

```text
[MSG] Sending: "Hi Bob, how are you?"
[AES] Encrypting with Shared Secret
[AES] IV:  B306590FA471D0672033E695E6129624
[AES] Ciphertext Generated (32 bytes)
[NET] Packet Sent
```

### Failed Verification (Tampered File)

```text
[ECDSA] START VERIFICATION PROCESS
[HASH]  Re-calculating SHA-256 Digest of original file...
[HASH]  Current Digest: 179AAC8D6D7E... 
[RESULT] INVALID. The file content does not match the signature.
```

---

## ⚠️ Disclaimer

This application is intended for **educational purposes only**. While it uses standard cryptographic libraries (`System.Security.Cryptography`), the implementation details (such as protocol design and key storage) are simplified for demonstration and may not be suitable for production environments requiring high-security standards.

---

## 📄 License
Distributed under the MIT License. See `LICENSE` for more information.

---

## 👨‍💻 Author
**Todoran C. Bogdan**
