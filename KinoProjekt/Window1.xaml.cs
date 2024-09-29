    using KinoProjekt.Pages;
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
        private int loggedInUserId;

        public Window1(int loggedInUser)
        {
            InitializeComponent();
            loggedInUserId = loggedInUser;
            adminSettings();
            navframe.Navigate(Home.NavLink);
            Home.IsSelected = true;
        }

        private void adminSettings()
        {
            if (loggedInUserId == 1)
            {
                Favourites.Visibility = Visibility.Collapsed;
                AdminPanel.Visibility = Visibility.Visible;
                
            }
            else
            {
                Favourites.Visibility = Visibility.Visible;
                AdminPanel.Visibility = Visibility.Collapsed;
            }
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
                Page page = null;
                switch (selected.Name)
                {
                    case "Account":
                        page = new KinoProjekt.Pages.Page1(this);
                        break;
                    case "Home":
                        page = new KinoProjekt.Pages.Page2(this);
                        break;
                    case "Favourites":
                        page = new KinoProjekt.Pages.Page3(this);
                        break;
                    case "AdminPanel":
                        page = new KinoProjekt.Pages.Page3(this);
                        break;
                    case "Upcoming":
                        page = new KinoProjekt.Pages.Page4();
                        break;
                    case "Info":
                        page = new KinoProjekt.Pages.Page5();
                        break;
                }

                navframe.NavigationService.Navigate(page);
            }
        }

        public int getLoggedInUserId()
        {
            return this.loggedInUserId;
        }

        private void NavFrame_Loaded(object sender, RoutedEventArgs e)
        {
            double frameWidth = navframe.ActualWidth;
            Console.WriteLine($"Szerokość navframe: {frameWidth}");
        }

    }
}
