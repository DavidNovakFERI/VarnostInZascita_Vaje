using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Naloga4_DavidNovak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Metoda za izbiro prve datoteke
        private void SelectFirstFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                string filePath1 = openFileDialog1.FileName;
                FirstFilePath.Text = filePath1;
            }
        }

        // Metoda za izbiro druge datoteke
        private void SelectSecondFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            if (openFileDialog2.ShowDialog() == true)
            {
                string filePath2 = openFileDialog2.FileName;
                SecondFilePath.Text = filePath2;
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                FirstFilePath.Text = filePath;

                //Preberi izbrano zgoščevalno datoteko
                string selectedAlgorithm = ((ComboBoxItem)HashAlgorithmComboBox.SelectedItem).Content.ToString();

                //zgoščevanje datoteke
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                byte[] hashBytes;

                switch (selectedAlgorithm)
                {
                    case "MD5":
                        using (MD5 md5 = MD5.Create())
                        {
                            hashBytes = md5.ComputeHash(fileBytes);
                        }
                        break;
                    case "SHA1":
                        using (SHA1 sha1 = SHA1.Create())
                        {
                            hashBytes = sha1.ComputeHash(fileBytes);
                        }
                        break;
                    case "SHA256":
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            hashBytes = sha256.ComputeHash(fileBytes);
                        }
                        break;
                    case "bCrypt":
                        // Uporabniku omogoči, da vnese število iteracij
                        int iterations = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Vnesite število iteracij:", "Število iteracij"));
                        // Zdaj lahko uporabite BCrypt algoritem za zgoščevanje datoteke s številom iteracij
                        string hash = BCrypt.Net.BCrypt.HashPassword(Encoding.UTF8.GetString(fileBytes), iterations);
                        // Lahko shranite hash v bazo podatkov, napišete v datoteko itd.
                        MessageBox.Show("Datoteka je bila uspešno zgoščena z bCrypt algoritmom.");
                        return;
                    default:
                        MessageBox.Show("Izbrana zgoščevalna funkcija ni podprta.");
                        return;
                }

                string hashedValue = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                MessageBox.Show("Datoteka je bila uspešno zgoščena z " + selectedAlgorithm + " algoritmom. Hash: " + hashedValue);

                // Shranjevanje zgoščene vrednosti in izbrane zgoščevalne funkcije v tekstovno datoteko
                string savePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "zgoscene_vrednosti.txt");
                using (StreamWriter sw = File.AppendText(savePath))
                {
                    sw.WriteLine("File: " + filePath);
                    sw.WriteLine("Hash: " + hashedValue);
                    sw.WriteLine("Algorithm: " + selectedAlgorithm);
                    sw.WriteLine();
                    sw.WriteLine("--------------------------------------------------");
                    sw.WriteLine();
                }



            }
        }

        private void CheckIntegrity_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == true)
            {
                string filePath1 = openFileDialog1.FileName;

                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                if (openFileDialog2.ShowDialog() == true)
                {
                    string filePath2 = openFileDialog2.FileName;

                    // Preberite izbrani zgoščevalni algoritem
                    string selectedAlgorithm = ((ComboBoxItem)HashAlgorithmComboBox.SelectedItem).Content.ToString();

                    // Izračunajte zgoščeno vrednost prve datoteke
                    byte[] fileBytes1 = System.IO.File.ReadAllBytes(filePath1);
                    byte[] hashBytes1;
                    switch (selectedAlgorithm)
                    {
                        case "MD5":
                            using (MD5 md5 = MD5.Create())
                            {
                                hashBytes1 = md5.ComputeHash(fileBytes1);
                            }
                            break;
                        case "SHA1":
                            using (SHA1 sha1 = SHA1.Create())
                            {
                                hashBytes1 = sha1.ComputeHash(fileBytes1);
                            }
                            break;
                        case "SHA256":
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                hashBytes1 = sha256.ComputeHash(fileBytes1);
                            }
                            break;
                        case "bCrypt":
                            MessageBox.Show("bCrypt algoritem ni primeren za preverjanje integritete datotek.");
                            return;
                        default:
                            MessageBox.Show("Izbrana zgoščevalna funkcija ni podprta.");
                            return;
                    }
                    string hashValue1 = BitConverter.ToString(hashBytes1).Replace("-", "").ToLower();

                    // Izračunajte zgoščeno vrednost druge datoteke
                    byte[] fileBytes2 = System.IO.File.ReadAllBytes(filePath2);
                    byte[] hashBytes2;
                    switch (selectedAlgorithm)
                    {
                        case "MD5":
                            using (MD5 md5 = MD5.Create())
                            {
                                hashBytes2 = md5.ComputeHash(fileBytes2);
                            }
                            break;
                        case "SHA1":
                            using (SHA1 sha1 = SHA1.Create())
                            {
                                hashBytes2 = sha1.ComputeHash(fileBytes2);
                            }
                            break;
                        case "SHA256":
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                hashBytes2 = sha256.ComputeHash(fileBytes2);
                            }
                            break;
                        case "bCrypt":
                            MessageBox.Show("bCrypt algoritem ni primeren za preverjanje integritete datotek.");
                            return;
                        default:
                            MessageBox.Show("Izbrana zgoščevalna funkcija ni podprta.");
                            return;
                    }
                    string hashValue2 = BitConverter.ToString(hashBytes2).Replace("-", "").ToLower();

                    // Primerjajte zgoščene vrednosti
                    if (hashValue1 == hashValue2)
                    {
                        MessageBox.Show("Datoteki sta identični.");
                    }
                    else
                    {
                        MessageBox.Show("Datoteki nista identični.");
                    }
                }
            }
        }




    }
}