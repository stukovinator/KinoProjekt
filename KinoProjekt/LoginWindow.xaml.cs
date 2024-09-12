using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

        private void loginWLoginButton_Click(object sender, MouseButtonEventArgs e)
        {
            if(loginWLogin.Text.Length == 0)
            {
                loginWerrorText.Content = "LOGIN JEST PUSTY";
                loginWerror.Visibility = Visibility.Visible;
            }
            else
            {
                if(loginWPassword.Text.Length == 0)
                {
                    loginWerrorText.Content = "HASŁO JEST PUSTE";
                    loginWerror.Visibility = Visibility.Visible;
                }
                else
                {
                    string login = loginWLogin.Text;
                    string password = loginWPassword.Text;
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {
                        conn.Open();
                        string query = "SELECT ";
                    }
                }
            }
        }
    }
}
