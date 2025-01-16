using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace FrekvenčnaAnaliza
{
    public partial class MainWindow : Window
    {
        private Dictionary<char, int> encryptedFrequency;
        private Dictionary<char, int> referenceFrequency;
        private string decryptedText;
        private Dictionary<char, char> manualReplacements = new Dictionary<char, char>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private Dictionary<char, int> GetCharacterFrequency(string text)
        {
            Dictionary<char, int> frequencyMap = new Dictionary<char, int>();
            text = Regex.Replace(text, "[^a-zA-Z]", "").ToLower();
            foreach (char c in text)
            {
                if (frequencyMap.ContainsKey(c))
                    frequencyMap[c]++;
                else
                    frequencyMap[c] = 1;
            }
            return frequencyMap;
        }

        private void AnalyzeEncryptedText_Click(object sender, RoutedEventArgs e)
        {
            encryptedFrequency = GetCharacterFrequency(EncryptedTextBox.Text);
            DisplayFrequencyAnalysis(encryptedFrequency);
        }

        private void AnalyzeReferenceText_Click(object sender, RoutedEventArgs e)
        {
            referenceFrequency = GetCharacterFrequency(ReferenceTextBox.Text);
            DisplayFrequencyAnalysis(referenceFrequency);
        }

        private void DisplayFrequencyAnalysis(Dictionary<char, int> frequency)
        {
            string analysis = "Frequency Analysis:\n";
            foreach (var entry in frequency)
                analysis += $"Character: {entry.Key}, Frequency: {entry.Value}\n";
            MessageBox.Show(analysis);
        }

        private void PartiallyRevealText_Click(object sender, RoutedEventArgs e)
        {
            // Preverimo, ali smo izbrali referenčno besedilo za analizo
            if (referenceFrequency == null)
            {
                MessageBox.Show("Najprej analizirajte referenčno besedilo.");
                return;
            }

            // Preverimo, ali smo analizirali šifrirano besedilo
            if (encryptedFrequency == null)
            {
                MessageBox.Show("Najprej analizirajte šifrirano besedilo.");
                return;
            }

            // Dekriptiramo šifrirano besedilo
            decryptedText = DecryptText(EncryptedTextBox.Text);

            // Razvrstimo črke v šifriranem besedilu po pogostosti pojavljanja
            var sortedEncryptedFrequency = encryptedFrequency.OrderByDescending(x => x.Value);
            // Razvrstimo črke v referenčnem besedilu po pogostosti pojavljanja
            var sortedReferenceFrequency = referenceFrequency.OrderByDescending(x => x.Value);

            // Zamenjamo črke v šifriranem besedilu z najbolj pogostimi črkami iz referenčnega besedila
            foreach (var encryptedEntry in sortedEncryptedFrequency)
            {
                foreach (var referenceEntry in sortedReferenceFrequency)
                {
                    // Preverimo, ali je črka v šifriranem besedilu že zamenjana
                    if (!manualReplacements.ContainsKey(encryptedEntry.Key))
                    {
                        // Preverimo, ali je črka v referenčnem besedilu že zamenjana
                        if (!manualReplacements.ContainsValue(referenceEntry.Key))
                        {
                            // Zamenjamo črko
                            manualReplacements.Add(encryptedEntry.Key, referenceEntry.Key);
                            break;
                        }
                    }
                }
            }

            // Dekriptiramo besedilo z uporabo ročno zamenjanih črk
            string partiallyDecryptedText = DecryptText(EncryptedTextBox.Text);
            DecryptedTextBox.Text = partiallyDecryptedText;
        }

        private string DecryptText(string encryptedText)
        {
            string decrypted = "";
            foreach (char c in encryptedText)
            {
                if (manualReplacements.ContainsKey(c))
                    decrypted += manualReplacements[c];
                else
                    decrypted += c;
            }
            return decrypted;
        }

        private void ClearStoredReveal_Click(object sender, RoutedEventArgs e)
        {
            manualReplacements.Clear();
            DecryptedTextBox.Text = DecryptText(EncryptedTextBox.Text);
        }

        private void OpenEncryptedFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileAndDisplayContent(EncryptedTextBox);
        }

        private void OpenReferenceFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileAndDisplayContent(ReferenceTextBox);
        }

        private void OpenFileAndDisplayContent(TextBox textBox)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string fileContent = File.ReadAllText(fileName);
                textBox.Text = fileContent;
            }
        }

        private void SaveDecryptedFile_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(DecryptedTextBox.Text);
        }

        private void SaveToFile(string text)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                File.WriteAllText(fileName, text);
            }
        }

        private void ManualReplacementTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Preverimo, ali je vnos v polje za ročno zamenjavo črk pravilen
            if (ManualReplacementTextBox.Text.Length % 2 == 0)
            {
                manualReplacements.Clear(); // Počistimo prejšnje ročne zamenjave

                // Razbijemo vnos na pare črk in zamenjamo v slovarju ročnih zamenjav
                for (int i = 0; i < ManualReplacementTextBox.Text.Length; i += 2)
                {
                    char originalChar = ManualReplacementTextBox.Text[i];
                    char replacementChar = ManualReplacementTextBox.Text[i + 1];
                    manualReplacements[originalChar] = replacementChar;
                    manualReplacements[replacementChar] = originalChar; // Avtomatično zamenjamo tudi obratno
                }

                // Dekriptiramo besedilo z uporabo ročno zamenjanih črk
                string partiallyDecryptedText = DecryptText(EncryptedTextBox.Text);
                DecryptedTextBox.Text = partiallyDecryptedText;
            }
            else
            {
                MessageBox.Show("Prosimo, vnesite parne število črk za ročno zamenjavo (npr. 'a' z 't', 'b' z 'x', itd.).");
            }
        }

    }
}
