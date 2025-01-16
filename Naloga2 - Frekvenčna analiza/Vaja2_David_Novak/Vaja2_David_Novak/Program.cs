using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vaja2_David_Novak
{
    public class Program
    {
        static Dictionary<char, char> zamenjaveCrk = new Dictionary<char, char>();

        static void Main(string[] args)
        {
            string sifriranoBesedilo = PreberiSifriranoBesedilo("sifrirana_datoteka.txt");
            string referencnoBesedilo = PreberiSifriranoBesedilo("referencna_datoteka.txt");

            if (sifriranoBesedilo != null && referencnoBesedilo != null)
            {
                Dictionary<char, int> frekvenceReferencnega = FrekvencaCrk(referencnoBesedilo);

                Console.WriteLine("Frekvenčna analiza referenčnega besedila:");
                IzpisiFrekvence(frekvenceReferencnega);

                Console.WriteLine("\nDelno razkritje šifriranega besedila:");
                string delnoRazkritoBesedilo = DelnoRazkrij(sifriranoBesedilo, referencnoBesedilo);
                Console.WriteLine(delnoRazkritoBesedilo);

                Console.WriteLine("\n\nShranjevanje delno razkritega besedila v datoteko...");
                ShraniBesedilo(delnoRazkritoBesedilo, "delno_razkrito_besedilo.txt");

                Console.WriteLine("\nRočna zamenjava črk ključa:");
                RočnaZamenjavaCrk(sifriranoBesedilo);
            }
            else
            {
                Console.WriteLine("Napaka pri branju ene od datotek.");
            }

            Console.ReadLine();
        }

        static string PreberiSifriranoBesedilo(string imeDatoteke)
        {
            string sifriranoBesedilo;
            try
            {
                sifriranoBesedilo = File.ReadAllText(imeDatoteke);
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka pri branju datoteke: " + e.Message);
                return null;
            }

            return sifriranoBesedilo;
        }

        static Dictionary<char, int> FrekvencaCrk(string besedilo)
        {
            Dictionary<char, int> frekvence = new Dictionary<char, int>();

            foreach (char crka in besedilo)
            {
                if (frekvence.ContainsKey(crka))
                {
                    frekvence[crka]++;
                }
                else
                {
                    frekvence[crka] = 1;
                }
            }

            return frekvence;
        }

        static void IzpisiFrekvence(Dictionary<char, int> frekvence)
        {
            foreach (var pair in frekvence)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }

        static string DelnoRazkrij(string sifriranoBesedilo, string referencnoBesedilo)
        {
            Console.WriteLine("Izberite črko, ki jo želite razkriti:");
            char izbranaCrka = Console.ReadKey().KeyChar;
            Console.WriteLine();

            Dictionary<char, int> frekvenceReferencnega = FrekvencaCrk(referencnoBesedilo);
            int najpogostejsePojavitveReferencnega = frekvenceReferencnega[izbranaCrka];

            Dictionary<char, int> frekvenceSifriranega = FrekvencaCrk(sifriranoBesedilo);
            List<char> moznostiCrk = new List<char>();

            foreach (var pair in frekvenceSifriranega)
            {
                if (Math.Abs(pair.Value - najpogostejsePojavitveReferencnega) <= 5)
                {
                    moznostiCrk.Add(pair.Key);
                }
            }

            Console.WriteLine($"Možne razkritje črke '{izbranaCrka}':");
            foreach (char crka in moznostiCrk)
            {
                Console.WriteLine(crka);
            }

            // Predogled delno razkritega besedila
            Console.WriteLine("\nDelno razkrito besedilo:");
            StringBuilder sb = new StringBuilder();
            foreach (char crka in sifriranoBesedilo)
            {
                if (crka == izbranaCrka || moznostiCrk.Contains(crka))
                {
                    sb.Append(crka);
                }
                else
                {
                    sb.Append("_");
                }
            }

            return sb.ToString();
        }

        static void ShraniBesedilo(string besedilo, string imeDatoteke)
        {
            try
            {
                File.WriteAllText(imeDatoteke, besedilo);
                Console.WriteLine("Besedilo uspešno shranjeno v datoteko.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Napaka pri shranjevanju datoteke: " + e.Message);
            }
        }

        static void RočnaZamenjavaCrk(string sifriranoBesedilo)
        {
            Console.WriteLine("Vnesite črko, ki jo želite zamenjati:");
            char crka1 = Console.ReadKey().KeyChar;
            Console.WriteLine("\nVnesite črko, s katero želite zamenjati izbrano črko:");
            char crka2 = Console.ReadKey().KeyChar;
            Console.WriteLine();

            zamenjaveCrk[crka1] = crka2;
            zamenjaveCrk[crka2] = crka1;

            Console.WriteLine($"Zamenjava črk {crka1} in {crka2} uspešno izvedena.");

            // Izpis besedila po zamenjavi črk
            Console.WriteLine("\nBesedilo po zamenjavi črk:");
            foreach (char crka in sifriranoBesedilo)
            {
                if (zamenjaveCrk.ContainsKey(crka))
                {
                    Console.Write(zamenjaveCrk[crka]);
                }
                else
                {
                    Console.Write(crka);
                }
            }
        }
    }
}
