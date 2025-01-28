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
        }

        private void Keres_Click(object sender, RoutedEventArgs e)
        {
            string ido = $"{Ora.Text}:{Perc.Text}";
            var eredmeny = kolcsonzesek.FindAll(k => k.VizenVan(ido)).ToList();
            foreach (var item in eredmeny) KeresesEredmeny.Items.Add(item);
        }
    }
}