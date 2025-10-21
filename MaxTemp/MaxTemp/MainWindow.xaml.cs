using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaxTemp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Diese Routine (EventHandler des Buttons Auswerten) liest die Werte
        /// zeilenweise aus der Datei temps.csv aus, merkt sich den höchsten Wert
        /// und gibt diesen auf der Oberfläche aus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAuswerten_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                // Zugriff auf Datei erstellen
                fs = new FileStream(@"temps.csv", FileMode.Open);
                sr = new StreamReader(fs);

                // Anfangswerte setzen
                double maxTemp = double.MinValue;
                string maxZeile = "";
                int zeilen = 0;
                int erfolgreichGelesen = 0;

                string zeile;
                while ((zeile = sr.ReadLine()) != null)
                {
                    zeilen++;

                    // Wert nach letztem Komma extrahieren
                    int lastComma = zeile.LastIndexOf(',');
                    if (lastComma < 0)
                        continue; // keine gültige Zeile

                    string tempText = zeile.Substring(lastComma + 1).Trim();

                    // Versuch, den Text in eine Zahl zu konvertieren
                    if (double.TryParse(
                            tempText.Replace(',', '.'),
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out double temp))
                    {
                        erfolgreichGelesen++;

                        // Prüfen, ob größer als bisheriges Maximum
                        if (temp > maxTemp)
                        {
                            maxTemp = temp;
                            maxZeile = zeile;
                        }
                    }
                }

                // Höchstwert anzeigen

                lblAusgabe.Content = $"--{maxTemp} °C, ist die höchste Temperatur--";

   
            }
            catch (Exception ex)
            {

                MessageBox.Show("Gleich krachelt das Programm...");
                throw new Exception("peng");
            }
            finally
            {
                // Datei wieder freigeben
                if (sr != null) sr.Close();
                if (fs != null) fs.Close();
            }
        }

    }
}
