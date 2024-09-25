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
using Path = System.Windows.Shapes.Path;

namespace KinoProjekt.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy Page3.xaml
    /// </summary>
    /// 

    public partial class Page3 : Page
    {
        public byte[] PlakatBytes;
        private int _loggedInUserId;
        //private List<Border> borders;
        public Page3(Window1 window)
        {
            InitializeComponent();
            _loggedInUserId = window.getLoggedInUserId();
            generateView(_loggedInUserId);
        }

        private void generateView(int userId)
        {
            if (userId != 1)
            {
                page3UserView.Visibility = Visibility.Visible;
                page3AdminView.Visibility = Visibility.Collapsed;
                loadUserReservations(userId);
            }
            else
            {
                page3UserView.Visibility = Visibility.Collapsed;
                page3AdminView.Visibility = Visibility.Visible;
            }
        }

        private void loadUserReservations(int userId)
        {
            using (var db = new SqliteDbContext()){
                var reservations = db.Reservations
                    .Where(r => r.UzytkownikId == userId)
                    .Join(db.Screenings, r => r.SeansId, s => s.Id, (r, s) => new { r, s.FilmID, s.Data, r.NrSiedzenia })
                    .Join(db.Movies, rs => rs.FilmID, f => f.Id, (rs, f) => new { rs.r, f.Tytul, rs.NrSiedzenia, rs.Data })
                    .ToList();

                foreach (var reservation in reservations)
                {
                    var border = createReservation(reservation.Tytul, reservation.NrSiedzenia, reservation.Data, reservation.r.Id);
                    //borders.Add(border);
                    page3UserViewReservations.Children.Add(border);
                }
            }
        }

        private Border createReservation(string movieTitle, int seatNumber, string date, int reservationId)
        {
            var border = new Border
            {
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#0466C8"),
                BorderThickness = new Thickness(3),
                Margin = new Thickness(500, 10, 500, 0),
                CornerRadius = new CornerRadius(3),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var stackPanel = new StackPanel
            {
                Margin = new Thickness(2),
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var titleStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var path = new System.Windows.Shapes.Path
            {
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 32,
                Width = 32,
                Margin = new Thickness(15, 15, 0, 15),
                Fill = (Brush)new BrushConverter().ConvertFrom("#0466C8"),
                VerticalAlignment = VerticalAlignment.Center,
                Data = Geometry.Parse("M19 3H18V1H16V3H8V1H6V3H5C3.9 3 3 3.9 3 5V19C3 20.1 3.9 21 5 21H19C20.1 21 21 20.1 21 19V5C21 3.9 20.1 3 19 3ZM19 19H5V9H19V19ZM5 7V5H19V7H5ZM10.56 17.46L16.49 11.53L15.43 10.47L10.56 15.34L8.45 13.23L7.39 14.29L10.56 17.46Z")
            };

            var titleLabel = new Label
            {
                Content = movieTitle.ToUpper() + " - " + date,
                Margin = new Thickness(5, 0, 0 , 0),
                Foreground = (Brush)new BrushConverter().ConvertFrom("#0466C8"),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 24,
                FontWeight = FontWeights.Bold
            };

            titleStackPanel.Children.Add(path);
            titleStackPanel.Children.Add(titleLabel);

            stackPanel.Children.Add(titleStackPanel);

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var seatLabel = new Label
            {
                Content = "MIEJSCE: " + seatNumber,
                Margin = new Thickness(10, 0, 0, 0),
                Foreground = (Brush)new BrushConverter().ConvertFrom("#0466C8"),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 24,
                FontWeight = FontWeights.Bold
            };

            Grid.SetColumn(seatLabel, 0);

            var cancelBorder = new Border
            {
                Padding = new Thickness(10, 0, 10, 0),
                Background = (Brush)new BrushConverter().ConvertFrom("#dc3545"),
                CornerRadius = new CornerRadius(5),
                Cursor = Cursors.Hand,
                Margin = new Thickness(15, 10, 15, 10),
            };

            var cancelLabel = new Label
            {
                Content = "REZYGNUJ",
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };

            cancelBorder.Child = cancelLabel;
            cancelBorder.MouseLeftButtonDown += (s, e) => cancelReservation(reservationId, border);
            Grid.SetColumn(cancelBorder, 1);

            grid.Children.Add(seatLabel);
            grid.Children.Add(cancelBorder);

            stackPanel.Children.Add(grid);
            border.Child = stackPanel;

            return border;
        }

        private void cancelReservation(int reservationId, Border border)
        {
            var result = MessageBox.Show("Czy napewno chcesz zrezygnować z rezerwacji?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var db = new SqliteDbContext())
                {
                    var reservation = db.Reservations.FirstOrDefault(r => r.Id == reservationId);
                    if (reservation != null)
                    {
                        db.Reservations.Remove(reservation);
                        db.SaveChanges();
                    }

                    page3UserViewReservations.Children.Remove(border);
                }
            }
        }

        private byte[] convertImageToBytes(string imagePath)
        {
            var fileInfo = new FileInfo(imagePath);
            if (fileInfo.Length > 5000000)
            {
                throw new Exception("Plik jest za duży. Maksymalny rozmiar plakatu to 5MB");
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
