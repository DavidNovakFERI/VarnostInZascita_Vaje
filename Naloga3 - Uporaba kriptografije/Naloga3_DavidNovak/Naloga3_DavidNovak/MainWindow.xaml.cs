using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Naloga3_DavidNovak
{
    public partial class MainWindow : Window
    {
        private byte[] aesKey;
        private byte[] aesIV;

        private int selectedAESKeyLength = 128;
        private int selectedRSAKeyLength = 1024;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadSourceFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                MessageBox.Show("File loaded: " + filePath);
            }
        }

        private void LoadAndSaveEncryptedFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;
                string encryptedFilePath = inputFilePath + ".encrypted";

                if (comboBoxAlgorithm.SelectedItem != null)
                {
                    string selectedAlgorithm = (comboBoxAlgorithm.SelectedItem as ComboBoxItem).Content.ToString();

                    if (selectedAlgorithm == "AES")
                    {
                        aesKey = GenerateAESKey();
                        aesIV = GenerateAESIV();
                        EncryptFileAES(inputFilePath, encryptedFilePath, aesKey, aesIV);
                        MessageBox.Show("File encrypted and saved: " + encryptedFilePath);
                    }
                    else if (selectedAlgorithm == "RSA")
                    {
                        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(selectedRSAKeyLength))
                        {
                            RSAParameters publicKey = rsa.ExportParameters(false);
                            RSAParameters privateKey = rsa.ExportParameters(true);

                            SavePrivateKey(privateKey);

                            EncryptFileRSA(inputFilePath, encryptedFilePath, publicKey);
                            MessageBox.Show("File encrypted and saved: " + encryptedFilePath);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an algorithm.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select an algorithm.");
                }
            }
        }

        private void SaveDecryptedFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string inputFilePath = openFileDialog.FileName;
                string decryptedFilePath = inputFilePath + ".decrypted";

                if (aesKey != null && aesIV != null)
                {
                    DecryptFileAES(inputFilePath, decryptedFilePath, aesKey, aesIV);
                    MessageBox.Show("File decrypted and saved: " + decryptedFilePath);
                }
                else
                {
                    MessageBox.Show("AES Key and IV not generated. Cannot decrypt the file.");
                }
            }
        }

        private void EncryptFileAES(string inputFile, string outputFile, byte[] key, byte[] IV)
        {
            aesKey = key;
            aesIV = IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        private void DecryptFileAES(string inputFile, string outputFile, byte[] key, byte[] IV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }

        private void EncryptFileRSA(string inputFile, string outputFile, RSAParameters publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);

                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                {
                    byte[] dataToEncrypt = new byte[inputStream.Length];
                    inputStream.Read(dataToEncrypt, 0, dataToEncrypt.Length);

                    byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                    outputStream.Write(encryptedData, 0, encryptedData.Length);
                }
            }
        }

        private void DecryptFileRSA(string inputFile, string outputFile, RSAParameters privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(privateKey);

                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                {
                    byte[] encryptedData = new byte[inputStream.Length];
                    inputStream.Read(encryptedData, 0, encryptedData.Length);

                    byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
                    outputStream.Write(decryptedData, 0, decryptedData.Length);
                }
            }
        }

        private void SavePrivateKey(RSAParameters privateKey)
        {
            using (StreamWriter writer = new StreamWriter("private_key.txt"))
            {
                writer.WriteLine(Convert.ToBase64String(privateKey.D));
                writer.WriteLine(Convert.ToBase64String(privateKey.DP));
                writer.WriteLine(Convert.ToBase64String(privateKey.DQ));
                writer.WriteLine(Convert.ToBase64String(privateKey.Exponent));
                writer.WriteLine(Convert.ToBase64String(privateKey.InverseQ));
                writer.WriteLine(Convert.ToBase64String(privateKey.Modulus));
                writer.WriteLine(Convert.ToBase64String(privateKey.P));
                writer.WriteLine(Convert.ToBase64String(privateKey.Q));
            }
        }

        private RSAParameters LoadPrivateKey(string privateKeyFilePath)
        {
            RSAParameters privateKey = new RSAParameters();

            using (StreamReader reader = new StreamReader(privateKeyFilePath))
            {
                privateKey.D = Convert.FromBase64String(reader.ReadLine());
                privateKey.DP = Convert.FromBase64String(reader.ReadLine());
                privateKey.DQ = Convert.FromBase64String(reader.ReadLine());
                privateKey.Exponent = Convert.FromBase64String(reader.ReadLine());
                privateKey.InverseQ = Convert.FromBase64String(reader.ReadLine());
                privateKey.Modulus = Convert.FromBase64String(reader.ReadLine());
                privateKey.P = Convert.FromBase64String(reader.ReadLine());
                privateKey.Q = Convert.FromBase64String(reader.ReadLine());
            }

            return privateKey;
        }

        private byte[] GenerateAESKey()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return aes.Key;
            }
        }

        private byte[] GenerateAESIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }

        private void comboBoxKeyLength_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxKeyLength.SelectedItem;
            string selectedItemContent = selectedItem.Content.ToString();

            if (selectedItemContent.Contains("AES"))
            {
                if (selectedItemContent.Contains("128"))
                {
                    selectedAESKeyLength = 128;
                }
                else if (selectedItemContent.Contains("192"))
                {
                    selectedAESKeyLength = 192;
                }
                else if (selectedItemContent.Contains("256"))
                {
                    selectedAESKeyLength = 256;
                }
            }
            else if (selectedItemContent.Contains("RSA"))
            {
                if (selectedItemContent.Contains("1024"))
                {
                    selectedRSAKeyLength = 1024;
                }
                else if (selectedItemContent.Contains("2048"))
                {
                    selectedRSAKeyLength = 2048;
                }
            }
        }

        private void comboBoxAlgorithm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)comboBoxAlgorithm.SelectedItem;
            string selectedAlgorithm = selectedItem.Content.ToString();

            comboBoxKeyLength.Items.Clear(); // Počisti obstoječe možnosti v ComboBoxu

            if (selectedAlgorithm == "AES")
            {
                comboBoxKeyLength.Items.Add("128");
                comboBoxKeyLength.Items.Add("192");
                comboBoxKeyLength.Items.Add("256");
            }
            else if (selectedAlgorithm == "RSA")
            {
                comboBoxKeyLength.Items.Add("1024");
                comboBoxKeyLength.Items.Add("2048");
            }

            comboBoxKeyLength.SelectedIndex = 0; // Nastavi privzeto izbrano možnost
        }
    }
}