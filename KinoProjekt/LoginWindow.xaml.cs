using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// 
    public partial class LoginWindow : Window
    {
        public int loggedInUserId { get; set; }
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

            if (string.IsNullOrWhiteSpace(loginWPassword.Password))
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

            using (var db = new SqliteDbContext())
            {
                try
                {
                    var existingUser = db.Users.FirstOrDefault(user => user.Login == loginWLogin.Text);

                    if (existingUser == null || existingUser.Haslo != loginWPassword.Password)
                    {
                        ShowError("NIEPRAWIDŁOWY LOGIN LUB HASŁO");
                        return;
                    }

                    loggedInUserId = existingUser.Id;

                    Window1 window1 = new Window1(loggedInUserId);
                    this.Visibility = Visibility.Hidden;
                    window1.Show();
                }
                catch (DbUpdateException dbEx)
                {
                    MessageBox.Show("Błąd podczas aktualizacji bazy danych: " + dbEx.InnerException?.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd logowania: " + ex.Message);
                }
                finally
                {
                    db.Dispose();
                }
            }
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
