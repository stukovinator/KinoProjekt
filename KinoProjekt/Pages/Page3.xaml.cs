using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace KinoProjekt.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public BitmapImage bi;
        public Page3()
        {
            InitializeComponent();
        }

        private void wybierzOkladke_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plakat";
            openFileDialog.Filter = "Wspierane grafiki |*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if(openFileDialog.ShowDialog() == true)
            {
                bi =  new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
            }
        }

        private void dodajFilm_Click(object sender, RoutedEventArgs e)
        {
            plakat.Source = bi;
            tekst.Content = tytul.Text;
            tytul.Text = string.Empty;
        }
    }
}
