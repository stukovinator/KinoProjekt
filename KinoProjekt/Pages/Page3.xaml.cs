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
using System.IO;

namespace KinoProjekt.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Page3.xaml
    /// </summary>
    /// 
    

    //public partial class 

    public partial class Page3 : Page
    {
        public byte[] PlakatBytes;
        public Page3(Window1 window, int mode)
        {
            InitializeComponent();
            generateView(mode);
        }

        private void generateView(int mode)
        {
            if (mode == 1)
            {
                page3UserView.Visibility = Visibility.Visible;
                page3AdminView.Visibility = Visibility.Collapsed;
            }
            else
            {
                page3UserView.Visibility = Visibility.Collapsed;
                page3AdminView.Visibility = Visibility.Visible;
            }
        }

        private byte[] convertImageToBytes(string imagePath)
        {
            var fileInfo = new FileInfo(imagePath);
            if (fileInfo.Length > 5000000) // 5MB limit (możesz zmienić na odpowiednią wartość)
            {
                throw new Exception("Plik jest za duży. Maksymalny rozmiar plakatu to 5MB.");
            }
            return File.ReadAllBytes(imagePath);
        }

        private void wybierzOkladke_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plakat";
            openFileDialog.Filter = "Wspierane grafiki |*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                var bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
                plakat.Source = bitmapImage;

                PlakatBytes = convertImageToBytes(openFileDialog.FileName);
            }
        }

        private void dodajFilm_Click(object sender, RoutedEventArgs e)
        {
            if (PlakatBytes == null)
            {
                MessageBox.Show("Wybierz plakat przed dodaniem filmu");
                return;
            }

            using (var db = new SqliteDbContext())
            {
                try
                {
                    var existingMovie = db.Movies.FirstOrDefault(m => m.Tytul == tytul.Text);
                    if (existingMovie != null)
                    {
                        MessageBox.Show("Film o tym tytule już istnieje");
                        return;
                    }

                    var newMovie = new Movie()
                    {
                        Tytul = tytul.Text,
                        Ocena = 1.0,
                        IloscGlosow = 0,
                        Opis = "Nowy film!",
                        Plakat = PlakatBytes
                    };

                    db.Movies.Add(newMovie);
                    db.SaveChanges();

                    MessageBox.Show("Film dodany pomyślnie");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Błąd dodawania filmu: " + ex.Message);
                    Console.WriteLine(ex.Message);
                    if (ex.InnerException != null)
                    {   
                        Console.WriteLine(ex.InnerException.Message);
                    }
                }
                finally
                {
                    tytul.Text = "";
                    plakat.Source = null;
                    PlakatBytes = null;
                    db.Dispose();
                }
            }
        }
    }
}
