using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Logika interakcji dla klasy Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public int _loggedInUserId;
        private int currentScreeningId;
        private int selectedSeatNumber = -1;

        public Page2(Window1 window)
        {
            InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Pages/bg.png"));
            this.Background = imageBrush;
            _loggedInUserId = window.getLoggedInUserId();
            loadMovies();
        }

        public class MovieWithScreening
        {
            public Movie Movie { get; set; }
            public int SeansId { get; set; }
        }

        public List<MovieWithScreening> GetMoviesFromScreenings()
        {
            using (var db = new SqliteDbContext())
            {
                var screenings = db.Screenings.Include(s => s.Movie).ToList();

                return screenings.Select(s => new MovieWithScreening
                {
                    Movie = s.Movie,
                    SeansId = s.Id
                }).ToList();
            }
        }

        public List<int> GetReservedSeatsForScreening(int screeningId)
        {
            using (var db = new SqliteDbContext())
            {
                return db.Reservations
                         .Where(r => r.SeansId == screeningId)
                         .Select(r => r.NrSiedzenia)
                         .ToList();
            }
        }

        private void loadMovies()
        {
            var moviesWithScreenings = GetMoviesFromScreenings();

            foreach (var item in moviesWithScreenings)
            {
                var movie = item.Movie;
                var seansId = item.SeansId;

                var movieButton = new Button()
                {
                    Height = 300,
                    Width = 210,
                    Margin = new Thickness(5),
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent
                };

                if (movie.Plakat != null)
                {
                    try
                    {
                        using (var stream = new MemoryStream(movie.Plakat))
                        {
                            var image = new Image();
                            var bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.StreamSource = stream;
                            bitmap.EndInit();
                            image.Source = bitmap;
                            image.Stretch = Stretch.UniformToFill;

                            movieButton.Content = image;
                            movieButton.Tag = seansId.ToString();
                            movieButton.ToolTip = movie.Tytul.ToUpper();
                            movieButton.Click += screeningPoster_Click;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd ładowania obrazu: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Film {movie.Tytul} nie ma plakatu");
                }

                moviesGrid.Children.Add(movieButton);
            }
        }

        private void screeningPoster_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var screeningId = Convert.ToInt32(button.Tag);

            showScreening(screeningId);
        }

        private string getMovieTitleByScreeningId(int screeningId)
        {
            using (var db = new SqliteDbContext())
            {
                var screening = db.Screenings.Include(s => s.Movie)
                                              .FirstOrDefault(s => s.Id == screeningId);
                return screening?.Movie?.Tytul ?? "NIEZNANY FILM";
            }
        }

        private void showScreening(int screeningId)
        {
            currentScreeningId = screeningId;
            moviesGrid.Visibility = Visibility.Collapsed;
            screenPanel.Visibility = Visibility.Visible;
            main.Visibility = Visibility.Visible;
            if (_loggedInUserId == 1)
            {
                page2ReservationPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                page2ReservationPanel.Visibility = Visibility.Visible;
            }
            screenPanelText.Content += getMovieTitleByScreeningId(screeningId).ToUpper();
            generateSeats();
        }

        private void selectSeat(int seatNumber)
        {
            selectedSeatNumber = seatNumber;
            page2ChosenSeat.Content = $"WYBRANO MIEJSCE: {seatNumber}";
            page2Reserve.IsEnabled = true;
            page2Reserve.Background = (Brush)new BrushConverter().ConvertFrom("#0466C8");
        }

        private void generateSeats()
        {
            main.Children.Clear();
            var bc = new BrushConverter();
            var index = 1;

            var reservedSeats = GetReservedSeatsForScreening(currentScreeningId);

            for (int i = 0; i < 4; i++)
            {
                StackPanel sp = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                for (int j = 0; j < (i % 2 == 0 ? 14 : 15); j++)
                {
                    int seatIndex = index;

                    Border border = new Border()
                    {
                        BorderBrush = (Brush)bc.ConvertFrom("#0466C8"),
                        BorderThickness = new Thickness(3),
                        Height = 60,
                        Width = 65,
                        CornerRadius = new CornerRadius(2, 2, 15, 15),
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    Label label = new Label()
                    {
                        Content = seatIndex,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        Foreground = (Brush)bc.ConvertFrom("#0466C8"),
                        FontSize = 18
                    };

                    if (reservedSeats.Contains(seatIndex))
                    {
                        border.BorderBrush = (Brush)bc.ConvertFrom("#dc3545");
                        label.Foreground = (Brush)bc.ConvertFrom("#dc3545");
                        border.IsEnabled = false;
                    }
                    else
                    {
                        border.Background = Brushes.White;
                        border.MouseLeftButtonDown += (sender, e) => selectSeat(seatIndex);
                    }

                    border.Child = label;
                    sp.Children.Add(border);
                    index++;
                }

                main.Children.Add(sp);
            }
        }

        private void reserveSeat(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (selectedSeatNumber == -1)
            {
                MessageBox.Show("Nie wybrano miejsca");
                return;
            }

            try
            {
                using (var db = new SqliteDbContext())
                {
                    var existingReservation = db.Reservations
                        .Any(r => r.UzytkownikId == _loggedInUserId && r.SeansId == currentScreeningId);

                    if (existingReservation)
                    {
                        MessageBox.Show("Już zarezerwowałeś miejsce na ten seans");
                        return;
                    }

                    var newReservation = new Reservation
                    {
                        UzytkownikId = _loggedInUserId,
                        SeansId = currentScreeningId,
                        NrSiedzenia = selectedSeatNumber
                    };

                    db.Reservations.Add(newReservation);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                {
                    MessageBox.Show("To miejsce na tym seansie zostało już zarezerwowane");
                }
                else
                {
                    MessageBox.Show("Wystąpił błąd podczas rezerwacji");
                }
            }


            MessageBox.Show($"Zarezerwowano miejsce {selectedSeatNumber}");
            selectedSeatNumber = -1;
            page2Reserve.IsEnabled = false;
            page2Reserve.Background = Brushes.Gray;
            generateSeats();
        }
    }
}