using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace Project_BlackJack_FTurkanKaya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        int speler1;
        int kaarten;
        int kapitaal;
        int inzet;      
        int speler1Waarde;
        int waarde;
        int aantalKaarten;
        int round = 0;
        int gewonnenBedragSpeler;
        int gewonnenBedragBank;      
        Boolean geenKaartSpeler; 


        int somBank;
        int somSpeler;
        bool isSpeler;
        bool isBank;
        Random kaart = new Random();

        private StringBuilder sb;
        private StringBuilder sbank;
        private StringBuilder sstatus;

        private DispatcherTimer klok, timerDeelkaart;

        List<string> gebruiktKaarten = new List<string>();       
        List<string> soortKaart = new List<string>() { "Schoppen", "Klaveren", "Harten", "Ruiten" };
        List<string> nummerKaart = new List<string>() { "Aas", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Boer", "Dame", "Koning" };
        List<string> speelKaarten = new List<string>();
        List<string> kaartenHistoric = new List<string>();
        List<string> kaartSource = new List<string>()
        {

            "/NewFolder/Schoppen/Schoppen_Aas.png",
            "/NewFolder/Schoppen/Schoppen_2.png",
            "/NewFolder/Schoppen/Schoppen_3.png",
            "/NewFolder/Schoppen/Schoppen_4.png",
            "/NewFolder/Schoppen/Schoppen_5.png",
            "/NewFolder/Schoppen/Schoppen_6.png",
            "/NewFolder/Schoppen/Schoppen_7.png",
            "/NewFolder/Schoppen/Schoppen_8.png",
            "/NewFolder/Schoppen/Schoppen_9.png",
            "/NewFolder/Schoppen/Schoppen_10.png",
            "/NewFolder/Schoppen/Schoppen_Boer.png",
            "/NewFolder/Schoppen/Schoppen_Dame.png",
            "/NewFolder/Schoppen/Schoppen_Koning.png",
            "/NewFolder/Klaveren/Klaveren_Aas.png",
            "/NewFolder/Klaveren/Klaveren_2.png",
            "/NewFolder/Klaveren/Klaveren_3.png",
            "/NewFolder/Klaveren/Klaveren_4.png",
            "/NewFolder/Klaveren/Klaveren_5.png",
            "/NewFolder/Klaveren/Klaveren_6.png",
            "/NewFolder/Klaveren/Klaveren_7.png",
            "/NewFolder/Klaveren/Klaveren_8.png",
            "/NewFolder/Klaveren/Klaveren_9.png",
            "/NewFolder/Klaveren/Klaveren_10.png",
            "/NewFolder/Klaveren/Klaveren_Boer.png",
            "/NewFolder/Klaveren/Klaveren_Dame.png",
            "/NewFolder/Klaveren/Klaveren_Koning.png",
            "/NewFolder/Harten/Harten_Aas.png",
            "/NewFolder/Harten/Harten_2.png",
            "/NewFolder/Harten/Harten_3.png",
            "/NewFolder/Harten/Harten_4.png",
            "/NewFolder/Harten/Harten_5.png",
            "/NewFolder/Harten/Harten_6.png",
            "/NewFolder/Harten/Harten_7.png",
            "/NewFolder/Harten/Harten_8.png",
            "/NewFolder/Harten/Harten_9.png",
            "/NewFolder/Harten/Harten_10.png",
            "/NewFolder/Harten/Harten_Boer.png",
            "/NewFolder/Harten/Harten_Dame.png",
            "/NewFolder/Harten/Harten_Koning.png",
            "/NewFolder/Ruiten/Ruiten_Aas.png",
            "/NewFolder/Ruiten/Ruiten_2.png",
            "/NewFolder/Ruiten/Ruiten_3.png",
            "/NewFolder/Ruiten/Ruiten_4.png",
            "/NewFolder/Ruiten/Ruiten_5.png",
            "/NewFolder/Ruiten/Ruiten_6.png",
            "/NewFolder/Ruiten/Ruiten_7.png",
            "/NewFolder/Ruiten/Ruiten_8.png",
            "/NewFolder/Ruiten/Ruiten_9.png",
            "/NewFolder/Ruiten/Ruiten_10.png",
            "/NewFolder/Ruiten/Ruiten_Boer.png",
            "/NewFolder/Ruiten/Ruiten_Dame.png",
            "/NewFolder/Ruiten/Ruiten_Koning.png",

        };

        ListBoxItem kaartenList = new ListBoxItem();

        private Dictionary<string, string> kaartImage;
        public MainWindow()
        {
            InitializeComponent();

            klok = new DispatcherTimer();

            TimeSpan interval = new TimeSpan(0, 0, 0);

            klok.Tick += Klok_Tick;

            klok.Interval = TimeSpan.FromSeconds(1);
            klok.Interval = new TimeSpan(0, 0, 0, 1);

            klok.Start();

            timerDeelkaart = new DispatcherTimer();
            timerDeelkaart.Tick += TimerDeelkaart_Tick;
            timerDeelkaart.Interval = TimeSpan.FromSeconds(2);


            sb = new StringBuilder();
            sbank = new StringBuilder();
            sstatus = new StringBuilder();

            //kaartWaarde = new Dictionary<string, int>();
        }
        private void TimerDeelkaart_Tick(object sender, EventArgs e)
        {
            GeefKaart();
        }
        private void Klok_Tick(object sender, EventArgs e)
        {
            LblTijdstip.Content = DateTime.Now.ToLongTimeString();
        }
        private void Knoppen()
        {
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            BtnDouble.IsEnabled = false;
        }
        private void GeefKaart()

        {
            timerDeelkaart.Start();

            if (isSpeler == true)
            {
                kaarten = kaart.Next(0, speelKaarten.Count);
                ImageSpeler.Source = new BitmapImage(new Uri(kaartImage[speelKaarten[kaarten]], UriKind.Relative));
                ImageSpeler.Visibility = Visibility.Visible;
                geenKaartSpeler = gebruiktKaarten.Contains(speelKaarten[waarde]);

                if (geenKaartSpeler == false)
                {
                    gebruiktKaarten.Add(speelKaarten[kaarten]);
                }
                waarde = 0;
                if (speelKaarten[kaarten].Contains("2"))
                {
                    waarde = 2;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("3"))
                {
                    waarde = 3;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("4"))
                {
                    waarde = 4;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("5"))
                {
                    waarde = 5;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("6"))
                {
                    waarde = 6;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("7"))
                {
                    waarde = 7;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("8"))
                {
                    waarde = 8;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("9"))
                {
                    waarde = 9;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("10") || speelKaarten[kaarten].Contains("Boer") || speelKaarten[kaarten].Contains("Dame") || speelKaarten[kaarten].Contains("Koning"))
                {
                    waarde = 10;
                    somSpeler += waarde;
                }
                else if (speelKaarten[kaarten].Contains("Aas"))
                {
                    if (somSpeler <= 10)
                    {
                        waarde = 11;
                        somSpeler += waarde;
                    }
                    else if (somSpeler > 10)
                    {
                        waarde = 1;
                        somSpeler += waarde;
                    }
                }
                isSpeler = false;
            }
            else if (isBank == true)
            {
                kaarten = kaart.Next(0, speelKaarten.Count);

                geenKaartSpeler = gebruiktKaarten.Contains(speelKaarten[kaarten]);

                if (geenKaartSpeler == false)
                {
                    gebruiktKaarten.Add(speelKaarten[kaarten]);
                }


                waarde = 0;
                if (speelKaarten[kaarten].Contains("2"))
                {
                    waarde = 2;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("3"))
                {
                    waarde = 3;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("4"))
                {
                    waarde = 4;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("5"))
                {
                    waarde = 5;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("6"))
                {
                    waarde = 6;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("7"))
                {
                    waarde = 7;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("8"))
                {
                    waarde = 8;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("9"))
                {
                    waarde = 9;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("10") || speelKaarten[kaarten].Contains("Boer") || speelKaarten[kaarten].Contains("Dame") || speelKaarten[kaarten].Contains("Koning"))
                {
                    waarde = 10;
                    somBank += waarde;
                }
                else if (speelKaarten[kaarten].Contains("Aas"))
                {
                    if (somBank <= 10)
                    {
                        waarde = 11;
                        somBank += waarde;
                    }
                    else if (somBank > 10)
                    {
                        waarde = 1;
                        somBank += waarde;
                    }
                }
                isBank = false;
            }
        }
        private void Clear()
        {
            TxtInzet.Clear();
            sb.Clear();
            sbank.Clear();
            LbxSpeler.Items.Clear();
            LbxBank.Items.Clear();
            speelKaarten.Clear();
            LbxBank.Items.Clear();
            LbxSpeler.Items.Clear();
            KartenListBox.Items.Clear();
            gebruiktKaarten.Clear();

            waarde = 0;
            speler1Waarde = 0;           
            somBank = 0;
            somSpeler = 0;
            inzet = 0;
            kapitaal = 100;

            BtnStand.IsEnabled = false;
            BtnHit.IsEnabled = false;
            BtnDeel.IsEnabled = true;
            BtnDouble.IsEnabled = false;
            TxtInzet.IsEnabled = true;
            TxtKapitaal.IsEnabled = true;
            TxtInzet.Focus();
          
            TxtScoreBank.Text = somBank.ToString();
            TxtScoreSpeler.Text = somSpeler.ToString();
            TxtResultaat.Text = "";
            TxtAantalkaarten.Text = "52";
           
            TxtKapitaal.Text = kapitaal.ToString();
            TxtInzet.Text = "";
            ImageSpeler.Source = new BitmapImage(new Uri("/NewFolder/Speelkaarten.jpg", UriKind.Relative));
            ImageSpeler.Visibility = Visibility.Visible;
            ImageBank.Source = new BitmapImage(new Uri("/NewFolder/Speelkaarten.jpg", UriKind.Relative));
            ImageBank.Visibility = Visibility.Visible;
            ImageDouble.Source = new BitmapImage(new Uri("/NewFolder/Speelkaarten.jpg", UriKind.Relative));
            ImageBank.Visibility = Visibility.Visible;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BtnDeel.IsEnabled = true;
            BtnHit.IsEnabled = false;
            BtnStand.IsEnabled = false;
            TxtScoreBank.Text = "0";
            TxtScoreSpeler.Text = "0";
            kapitaal = 100;
            TxtInzet.Text = "";
            TxtKapitaal.Text = kapitaal.ToString();
            BtnDouble.IsEnabled = false;
            TxtAantalkaarten.Text = "52";
            TxtInzet.Focus();
        }
        private void Gewonnen()
        {
            TxtResultaat.Text = "G E W O N N E N";
            TxtResultaat.Foreground = Brushes.DarkGreen;

            kapitaal += 2 * inzet;
            gewonnenBedragSpeler += 2 * inzet;
            TxtKapitaal.Text = Convert.ToString(kapitaal);

            Knoppen();
        }
        private void Verloren()
        {
            TxtResultaat.Text = "V E R L O R E N";
            TxtResultaat.Foreground = Brushes.Red;
            kapitaal -= inzet;
            gewonnenBedragBank += inzet;
            TxtKapitaal.Text = Convert.ToString(kapitaal);

            Knoppen();
        }
        private void Push()
        {
            TxtResultaat.Text = "P U S H";
            TxtResultaat.Foreground = Brushes.Orange;

            Knoppen();
        }
        private void ScoreSpeler()
        {
            if (somSpeler > 21 && somBank < 21)
            {
                Verloren();             
            }
            else if (somSpeler == 21)
            {
                Gewonnen();              
            }
        }
        private void LblStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (var status in kaartenHistoric)
            {
                MessageBox.Show(status);
            }
        }
        private void Score()
        {
            if (somBank == 21 && somSpeler < 21)
            {
                Verloren();             
            }
            else if (somSpeler == 21 && somBank < 21)
            {
                Gewonnen();              
            }
            else if (somSpeler < 21 && somBank < 21)
            {
                if (somSpeler > somBank)
                {
                    Gewonnen();   
                }
                else if (somSpeler == somBank)
                {
                    Push();
                }
                else if (somSpeler < somBank)
                {
                    Verloren();
                }
                Knoppen();
            }
            else if (somBank > 21 && somSpeler < 21)
            {
                Gewonnen();          
            }
            else if (somSpeler == somBank)
            {
                Push();             
            }
        }
       //************************************************  KAARTEN DELEN  ********************************************************************************************************
        private void BtnDeel_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < soortKaart.Count; i++)
            {
                for (int x = 0; x < nummerKaart.Count; x++)
                {
                    string nieuweKaart = soortKaart[i] + " " + nummerKaart[x];
                    speelKaarten.Add(nieuweKaart);
                    KartenListBox.Items.Add(nieuweKaart);
                }
            }

            kaartImage = new Dictionary<string, string>();

            for (int i = 0; i < kaartSource.Count; i++)
            {
                kaartImage.Add(speelKaarten[i], kaartSource[i]);
            }

            BtnHit.IsEnabled = true;
            BtnStand.IsEnabled = true;
            TxtInzet.IsEnabled = false;
            TxtKapitaal.IsEnabled = false;
         
            if (kapitaal == 0)
            {
                MessageBox.Show("U hebt geen voldoende kapitaal. U kan niet verder spelen. ");
            }
            if (string.IsNullOrWhiteSpace(TxtInzet.Text))
            {
                MessageBox.Show("U moest minimum inzet die 10% van het kapitaal is,bepalen.");
                inzet = kapitaal * 10 / 100;
                TxtInzet.Text = inzet.ToString();
            }
            else
            {
                inzet = Convert.ToInt16(TxtInzet.Text);
                if (inzet < kapitaal * 10 / 100)
                {
                    MessageBox.Show("U moet minimum inzet die 10% van het kapitaal is,bepalen.");
                    inzet = kapitaal * 10 / 100;
                    TxtInzet.Text = inzet.ToString();
                }
            }

            SldKapitaal.Value = kapitaal;
            TxtInzet.Text = inzet.ToString();

            TxtKapitaal.Text = Convert.ToString(kapitaal);

            TxtInzet.Text = Convert.ToString(inzet);
          
            isSpeler = true;
            isBank = false;
            GeefKaart();

            LbxSpeler.Items.Add(speelKaarten[kaarten]);

            KartenListBox.Items.Remove(speelKaarten[kaarten]);
            speelKaarten.Remove(speelKaarten[kaarten]);
           
            speler1 = kaart.Next(0, speelKaarten.Count);

            ImageSpeler.Source = new BitmapImage(new Uri(kaartImage[speelKaarten[speler1]], UriKind.Relative));
            ImageSpeler.Visibility = Visibility.Visible;

            speler1Waarde = 0;
            if (speelKaarten[speler1].Contains("2"))
            {
                speler1Waarde = 2;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("3"))
            {
                speler1Waarde = 3;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("4"))
            {
                speler1Waarde = 4;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("5"))
            {
                speler1Waarde = 5;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("6"))
            {
                speler1Waarde = 6;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("7"))
            {
                speler1Waarde = 7;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("8"))
            {
                speler1Waarde = 8;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("9"))
            {
                speler1Waarde = 9;
                somSpeler += speler1Waarde;
            }
            else if (speelKaarten[speler1].Contains("10") || speelKaarten[speler1].Contains("Boer") || speelKaarten[speler1].Contains("Dame") || speelKaarten[speler1].Contains("Koning"))
            {
                speler1Waarde = 10;
                somSpeler += speler1Waarde;

            }
            else if (speelKaarten[speler1].Contains("Aas"))
            {
                if (somSpeler <= 10)
                {
                    speler1Waarde = 11;
                    somSpeler += speler1Waarde;
                }
                else if (somSpeler > 10)
                {
                    speler1Waarde = 1;
                    somSpeler += speler1Waarde;
                }
            }

            LbxSpeler.Items.Add(speelKaarten[speler1]);
            TxtScoreSpeler.Text = somSpeler.ToString();

            geenKaartSpeler = gebruiktKaarten.Contains(speelKaarten[speler1]);

            if (geenKaartSpeler == false)
            {
                gebruiktKaarten.Add(speelKaarten[speler1]);
            }

            KartenListBox.Items.Remove(speelKaarten[speler1]);
            speelKaarten.Remove(speelKaarten[speler1]);
            aantalKaarten = speelKaarten.Count;
            TxtAantalkaarten.Text = aantalKaarten.ToString();

            // ***********************************************  BANK KAARTEN  *********************************************************************

            isSpeler = false;
            isBank = true;

            GeefKaart();
            ImageBank.Source = new BitmapImage(new Uri(kaartImage[speelKaarten[kaarten]], UriKind.Relative));
            ImageBank.Visibility = Visibility.Visible;
           
            LbxBank.Items.Add(speelKaarten[kaarten]);
            TxtScoreBank.Text = somBank.ToString();

            KartenListBox.Items.Remove(speelKaarten[kaarten]);
            speelKaarten.Remove(speelKaarten[kaarten]);

            aantalKaarten = speelKaarten.Count;
            TxtAantalkaarten.Text = aantalKaarten.ToString();

            ScoreSpeler();

            if (somSpeler != 0 && inzet * 2 <= kapitaal)
            {
                BtnDouble.IsEnabled = true;
            }
          
            BtnDeel.IsEnabled = false;
        }

        //**************************************************  DOUBLE DOWN   *****************************************************************
        
        private void BtnDouble_Click(object sender, RoutedEventArgs e)
        {
            isSpeler = false;
            isBank = true;
            GeefKaart();
            LbxBank.Items.Add(speelKaarten[kaarten]);

            ImageDouble.Source = new BitmapImage(new Uri(kaartImage[speelKaarten[kaarten]], UriKind.Relative));
            ImageDouble.Visibility = Visibility.Visible;

            inzet = 2 * inzet;
            TxtInzet.Text = inzet.ToString();

            MessageBox.Show("De Double Down button niet meer beshickbaar");

            TxtScoreBank.Text = somBank.ToString();
            KartenListBox.Items.Remove(speelKaarten[kaarten]);
            speelKaarten.Remove(speelKaarten[kaarten]);
            aantalKaarten = speelKaarten.Count;
            TxtAantalkaarten.Text = aantalKaarten.ToString();

            BtnHit.IsEnabled = false;
            BtnDouble.IsEnabled = false;

            ScoreSpeler();
        }
        private void BtnHit_Click(object sender, RoutedEventArgs e)
        {
            BtnDouble.IsEnabled = false;

            isBank = false;
            isSpeler = true;

            GeefKaart();
            LbxSpeler.Items.Add(speelKaarten[kaarten]);
            KartenListBox.Items.Remove(speelKaarten[kaarten]);
            speelKaarten.Remove(speelKaarten[kaarten]);
            TxtScoreSpeler.Text = somSpeler.ToString();

            aantalKaarten = speelKaarten.Count;
            TxtAantalkaarten.Text = aantalKaarten.ToString();

            ScoreSpeler();
        }

        private void BtnStand_Click(object sender, RoutedEventArgs e)
        {
            if (somBank < 16)
            {
                do
                {
                    isSpeler = false;
                    isBank = true;

                    GeefKaart();
                    sbank.AppendLine($"{speelKaarten[kaarten]}");
                   
                } while (somBank < 16);

                if (somBank >= 16)
                {
                    isBank = false;
                }
                LbxBank.Items.Add(sbank);
            }
            else
            {
                GeefKaart();
                LbxBank.Items.Add(speelKaarten[kaarten]);
                TxtScoreBank.Text = somBank.ToString();
            }

            TxtScoreBank.Text = somBank.ToString();
            ImageBank.Source = new BitmapImage(new Uri(kaartImage[speelKaarten[kaarten]], UriKind.Relative));
            ImageBank.Visibility = Visibility.Visible;

            KartenListBox.Items.Remove(speelKaarten[kaarten]);
            speelKaarten.Remove(speelKaarten[kaarten]);

            aantalKaarten = speelKaarten.Count;
            TxtAantalkaarten.Text = aantalKaarten.ToString();

            Score();
        }
        private void BtnNieuwSpeel_Click(object sender, RoutedEventArgs e)
        {
            sstatus.Clear();
            round++;

            sstatus.AppendLine($"{round}.Round / Gewonnen Bedrag: {gewonnenBedragSpeler} - Speler: {somSpeler} / Bank: {somBank}");
            kaartenHistoric.Add(Convert.ToString(sstatus));

            if (kaartenHistoric.Count > 10)
            {
                kaartenHistoric.Remove(kaartenHistoric[0]);
            }

            LblStatus.Content = sstatus.ToString();


            // ***********************************************************  SHUFFLE DE DECK  ******************************************************************


            MessageBox.Show("Het deck geschud wordt");

            Random shuffle = new Random();
            for (int i = 0; i < speelKaarten.Count; i++)
            {
                int nieuweDeck = shuffle.Next(i, speelKaarten.Count);
                string temp = speelKaarten[i];
                speelKaarten[i] = speelKaarten[nieuweDeck];
                speelKaarten[nieuweDeck] = temp;
            }
            Clear();
        }
    }
}
    


