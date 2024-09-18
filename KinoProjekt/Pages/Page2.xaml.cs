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
    /// Logika interakcji dla klasy Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        //public List<Film> films = new List<Film>();
        public Page2()
        {
            InitializeComponent();
            generateSeats();
            //films.Add(new Film(new BitmapImage(new Uri("../plakat.jpg", UriKind.RelativeOrAbsolute)), "Jaws"));

            //Image image = new Image
            //{
            //    Source = films[0].poster
            //};
            //Label label = new Label
            //{
            //    Content = films[0].title
            //};
            //StackPanel stackPanel = new StackPanel
            //{
            //    Orientation = Orientation.Horizontal
            //};

            //stackPanel.Children.Add(image);
            //stackPanel.Children.Add(label);

            //filmy.Children.Add(stackPanel);
        }

        public void generateSeats()
        {
            var bc = new BrushConverter();
            var index = 1;

            for(int i = 0; i<5; i++)
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