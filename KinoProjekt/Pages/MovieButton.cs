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
    public class MovieButton : ListBoxItem
    {
        static MovieButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MovieButton), new FrameworkPropertyMetadata(typeof(MovieButton)));
        }

        public ImageSource Poster
        {
            get { return (ImageSource)GetValue(PosterProperty); }
            set { SetValue(PosterProperty, value); }
        }
        public static readonly DependencyProperty PosterProperty = DependencyProperty.Register("Poster", typeof(ImageSource), typeof(MovieButton), new PropertyMetadata(null));


    }
}
