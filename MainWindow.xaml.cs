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

            for(int i = 0; i < 24; i++)
            {
                Ora.Items.Add(i);
                FelvitelKezdesOra.Items.Add(i);
                FelvitelVegeOra.Items.Add(i);
            }
            for (int i = 0; i < 60; i++)
            {
                Perc.Items.Add(i);
                FelvitelKezdesPerc.Items.Add(i);
                FelvitelVegePerc.Items.Add(i);
            }

            for (int i = 1; i < 11; i++) FelvitelSzemelyekSzama.Items.Add(i);

            FelvitelAzon.ItemsSource = kolcsonzesek.Select(k => k.HajoAzonosito).Distinct().ToList();

            FelvitelHajoTipus.Items.Add("kajak");
            FelvitelHajoTipus.Items.Add("kenu");

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

        private bool HajoKereso(int Azon, string Tipus, int Személyes)
        {
            var letezik = kolcsonzesek.Where(k => k.HajoAzonosito.Equals(Azon) && k.HajoTipusa.Equals(Tipus) && k.SzemelyekSzama.Equals(Személyes)).ToList();
            bool eredmeny = (letezik.Count() > 0 ? true : false);
            return eredmeny;
        }

        private void SerultKereses()
        {
            var serultek = kolcsonzesek.Where(k => k.HajoAzonosito.Equals(int.Parse(SerultAzon.Text))).ToList();
            using StreamWriter sw = new(
                path: @$"../../../src/rongalas_{SerultAzon.Text}.txt",
                append: false);
            foreach (var item in serultek) sw.WriteLine($"{item.Nev} - { item.KolcsonzesIdotartam()}");
            SerultAzon.Text = "";
        }

        private void SerultKeres_Click(object sender, RoutedEventArgs e)
        {
            SerultKereses();
        }

        private void Felvitel_Click(object sender, RoutedEventArgs e)
        {
            if (HajoKereso(int.Parse(FelvitelAzon.Text) , FelvitelHajoTipus.Text, int.Parse(FelvitelSzemelyekSzama.Text)))
            {
                kolcsonzesek.Add(new Kolcsonzes($"{FelvitelNev.Text};{FelvitelAzon.Text};{FelvitelHajoTipus.Text};{int.Parse(FelvitelSzemelyekSzama.Text)};{int.Parse(FelvitelKezdesOra.Text)};{int.Parse(FelvitelKezdesPerc.Text)};{int.Parse(FelvitelVegeOra.Text)};{int.Parse(FelvitelVegePerc.Text)}"));
                Kolcsonzesek_Grid.ItemsSource = kolcsonzesek;
            }
            else MessageBox.Show("Nincs ilyen hajó!");
        }
    }
}