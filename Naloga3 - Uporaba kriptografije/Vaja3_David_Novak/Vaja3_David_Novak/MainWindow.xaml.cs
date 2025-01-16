using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Vaja3_David_Novak
{
    public partial class MainWindow : Window
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; NotifyPropertyChanged(); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Metoda za nalaganje izvorne datoteke
        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
        }

        // Metoda za obveščanje o spremembah lastnosti
        private event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda za šifriranje datoteke z AES
        private void EncryptFileWithAES(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                {
                    using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create))
                    {
                        fsEncrypted.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                        using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = fsInput.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    csEncrypt.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Metoda za dešifriranje datoteke z AES
        private void DecryptFileWithAES(string inputFile, string outputFile, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
                {
                    byte[] fileIv = new byte[aesAlg.BlockSize / 8];
                    fsInput.Read(fileIv, 0, fileIv.Length);

                    using (FileStream fsOutput = new FileStream(outputFile, FileMode.Create))
                    {
                        using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fsOutput.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                }
            }
        }

        // Metoda za šifriranje in shranjevanje datoteke
        private void EncryptAndSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string inputFile = openFileDialog.FileName;
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() == true)
                {
                    string outputFile = saveFileDialog.FileName;
                    byte[] aesKey = GenerateAESKey();
                    EncryptFileWithRSA(inputFile, outputFile, aesKey);
                    MessageBox.Show("File encrypted and saved!");
                }
            }
        }

        // Metoda za dešifriranje in shranjevanje datoteke
        private void DecryptAndSave_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string inputFile = openFileDialog.FileName;
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                if (saveFileDialog.ShowDialog() == true)
                {
                    string outputFile = saveFileDialog.FileName;
                    byte[] aesKey = DecryptAESKeyWithRSA(inputFile);
                    byte[] iv = GetIVFromFile(inputFile); // Dobi IV iz datoteke ali drugega vira
                    DecryptFileWithAES(inputFile, outputFile, aesKey, iv); // Dodaj iv kot argument
                    MessageBox.Show("File decrypted and saved!");
                }
            }
        }

        // Metoda za pridobivanje IV iz datoteke
        private byte[] GetIVFromFile(string inputFile)
        {
            byte[] iv;
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            {
                iv = new byte[16];
                fsInput.Read(iv, 0, iv.Length);
            }
            return iv;
        }

        // Metoda za generiranje naključnega AES ključa
        private byte[] GenerateAESKey()
        {
            string selectedAesKey = ((ComboBoxItem)AesKeyLengthComboBox.SelectedItem).Content.ToString();
            switch (selectedAesKey)
            {
                case "AES-128":
                    return GenerateRandomKey(128);
                case "AES-192":
                    return GenerateRandomKey(192);
                case "AES-256":
                    return GenerateRandomKey(256);
                default:
                    throw new ArgumentException("Invalid AES key size selected.");
            }
        }

        // Metoda za generiranje naključnega ključa
        private byte[] GenerateRandomKey(int keySize)
        {
            byte[] key = new byte[keySize / 8];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        // Metoda za šifriranje datoteke z RSA
        private void EncryptFileWithRSA(string inputFile, string outputFile, byte[] aesKey)
        {
            using (RSA rsa = RSA.Create(GetRSAKeySize()))
            {
                byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
                using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create))
                {
                    fsEncrypted.Write(encryptedAesKey, 0, encryptedAesKey.Length);
                    EncryptFileWithAES(inputFile, outputFile, aesKey, rsa.ExportParameters(false).Modulus);
                }
            }
        }

        // Metoda za dešifriranje AES ključa z RSA
        private byte[] DecryptAESKeyWithRSA(string inputFile)
        {
            byte[] encryptedAesKey;
            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            {
                encryptedAesKey = new byte[GetRSAKeySize() / 8];
                fsInput.Read(encryptedAesKey, 0, encryptedAesKey.Length);
            }
            using (RSA rsa = RSA.Create(GetRSAKeySize()))
            {
                // Import private key from file or other source
                // rsa.ImportRSAPrivateKey(/* Load private key from file */);
                return rsa.Decrypt(encryptedAesKey, RSAEncryptionPadding.OaepSHA256);
            }
        }

        // Metoda za pridobitev dolžine RSA ključa
        private int GetRSAKeySize()
        {
            string selectedRsaKey = ((ComboBoxItem)RsaKeyLengthComboBox.SelectedItem).Content.ToString();
            switch (selectedRsaKey)
            {
                case "RSA-1024":
                    return 1024;
                case "RSA-2048":
                    return 2048;
                default:
                    throw new ArgumentException("Invalid RSA key size selected.");
            }
        }
    }
}
