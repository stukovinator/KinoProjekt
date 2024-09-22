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
        public Page2(Window1 window)
        {
            InitializeComponent();
            //generateSeats();
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

        private void loadMovies()
        {
            Console.WriteLine("Start");
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
                            movieButton.ToolTip = $"SEANS ID: {seansId}";

                            Console.WriteLine($"Dodano film: {movie.Tytul}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Błąd ładowania obrazu: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Film {movie.Tytul} nie ma plakatu.");
                }

                moviesGrid.Children.Add(movieButton);
            }
        }


        private void generateSeats()
        {
            var bc = new BrushConverter();
            var index = 1;

            for (int i = 0; i < 5; i++)
            {
                StackPanel sp = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                if (i % 2 != 0)
                {
                    for (int j = 0; j < 20; j++)
                    {
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
                            Content = index,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontFamily = new FontFamily("/Fonts/#Montserrat Bold"),
                            FontSize = 18
                        };

                        border.Child = label;
                        sp.Children.Add(border);
                        index++;
                    }
                    main.Children.Add(sp);
                }
                else
                {
                    for (int j = 0; j < 19; j++)
                    {
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
                            Content = index,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontFamily = new FontFamily("/Fonts/#Montserrat Bold"),
                            FontSize = 18
                        };

                        border.Child = label;
                        sp.Children.Add(border);
                        index++;
                    }
                    main.Children.Add(sp);
                }
            }
        }


    }
}