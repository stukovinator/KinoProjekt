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
        public List<Film> films = new List<Film>();
        public Page2()
        {
            InitializeComponent();
            films.Add(new Film(new BitmapImage(new Uri("../plakat.jpg", UriKind.RelativeOrAbsolute)), "Jaws"));

            Image image = new Image
            {
                Source = films[0].poster
            };
            Label label = new Label
            {
                Content = films[0].title
            };
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(label);

            filmy.Children.Add(stackPanel);
        }
    }

    public class Film
    {
        public BitmapImage poster;
        public string title;

        public Film()
        {
            poster = null;
            title = null;
        }

        public Film(BitmapImage poster, string title)
        {
            this.poster = poster;
            this.title = title;
        }
    }
}