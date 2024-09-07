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
using System.Windows.Shapes;

namespace KinoProjekt
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            navframe.Navigate(Home.NavLink);
            Home.IsSelected = true;
        }

        private void NavBarSelect(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton;

            if (selected.Name == "Shutdown")
            {
                Application.Current.Shutdown();
            }
            else
            {
                navframe.Navigate(selected.NavLink);
            }


        }
    }
}
