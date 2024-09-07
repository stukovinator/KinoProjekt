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

namespace KinoProjekt
{
    public class ExitButton : ListBoxItem
    {
        static ExitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExitButton), new FrameworkPropertyMetadata(typeof(ExitButton)));
        }
        public Geometry Icon
        {
            get { return (Geometry)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Geometry), typeof(ExitButton), new PropertyMetadata(null));
    }
}
