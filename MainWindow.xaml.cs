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
using System.IO;
using Kajak_Kenu_Grafikus;


namespace Kajak_Kenu_Grafikus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Kolcsonzes> kolcsonzesek = new();
        public MainWindow()
        {
            InitializeComponent();

            using StreamReader sr = new(
                path: @"../../../src/kolcsonzes.txt",
                encoding: Encoding.UTF8);
            _ = sr.ReadLine();
            while (!sr.EndOfStream) kolcsonzesek.Add(new(sr.ReadLine()));
            foreach(var kolcsonzes in kolcsonzesek) Kolcsonzesek_Grid.Items.Add(kolcsonzes);

            for(int i = 0; i < 24; i++) Ora.Items.Add(i);
            for (int i = 0; i < 60; i++) Perc.Items.Add(i);

            foreach (var kolcsonzes in kolcsonzesek) f13.Items.Add(kolcsonzes.ToString());

            KolcsonzesekMara.Visibility = Visibility.Hidden;
        }

        private void Keres_Click(object sender, RoutedEventArgs e)
        {
            string ido = $"{Ora.Text}:{Perc.Text}";
            var eredmeny = kolcsonzesek.FindAll(k => k.VizenVan(ido)).ToList();
            foreach (var item in eredmeny) KeresesEredmeny.Items.Add(item);
        }

        private void NapiBevetel_Click(object sender, RoutedEventArgs e)
        {
            NapiBevetelOsszeg.Content = kolcsonzesek.Sum(k => k.MegkezdettFelOrak()) * 1500 + " Ft";
        }

        private void Statisztika_Click(object sender, RoutedEventArgs e)
        {
            var Adatok = kolcsonzesek.OrderBy(k => k.HajoAzonosito).GroupBy(k => k.HajoAzonosito).ToDictionary(k => k.Key, k => k.Count());
            using StreamWriter sw = new(
                path: @"../../../src/statisztika.txt",
                append: false);
            foreach (var item in Adatok)
            {
                sw.WriteLine($"{item.Key} - {item.Value}");
                KolcsonzesekMara.Items.Add(item);
            }
            KolcsonzesekMara.Visibility = Visibility.Visible;
        }

        public bool HajotKeres_Click(object sender, RoutedEventArgs e)
        {
            var letezik = kolcsonzesek.Where(k => k.HajoAzonosito.Equals(Azon.Text) && k.HajoTipusa.Equals(Tipus.Text) && k.SzemelyekSzama.Equals(Személyes)).ToList();
            bool eredmeny = (letezik.Count() > 0 ? true : false);
            return eredmeny;
        }

        private void SerultKeres_Click(object sender, RoutedEventArgs e)
        {
            var serult = kolcsonzesek.Where(k => k.HajoAzonosito.Equals(SerultAzon.Text)).ToList();
            if(serult.Count() != 0)
            {
                using StreamWriter sw = new(
                    path: $@"../../../src/rongalas_{SerultAzon.Text}.txt",
                    append: false);
                foreach (var item in serult) sw.WriteLine($"{item.Nev} - {item.KolcsonzesIdotartam()}");
            }
        }
    }
}