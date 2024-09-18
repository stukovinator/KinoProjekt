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
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string dbPath = "DataSource=C:/Users/denic/source/repos/KinoProjekt/KinoProjekt/KinoProjekt.db;Version=3;";
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ShowError(string message)
        {
            loginWerrorText.Content = message;
            loginWerror.Visibility = Visibility.Visible;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(loginWLogin.Text))
            {
                ShowError("LOGIN JEST PUSTY");
                return false;
            }

            if (string.IsNullOrWhiteSpace(loginWPassword.Text))
            {
                ShowError("HASŁO JEST PUSTE");
                return false;
            }

            return true;
        }

        private void loginWLoginButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            Window1 window1 = new Window1();
            this.Visibility = Visibility.Hidden;
            window1.Show();
        }

        private void loginWBack_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
            this.Close();
        }
    }
}
