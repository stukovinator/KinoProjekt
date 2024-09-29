using KinoProjekt.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace KinoProjekt
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    /// 

    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void ShowError(string message)
        {
            registerWerrorText.Content = message;
            registerWerror.Visibility = Visibility.Visible;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(registerWLogin.Text))
            {
                ShowError("LOGIN JEST PUSTY");
                return false;
            }

            if (string.IsNullOrWhiteSpace(registerWEmail.Text))
            {
                ShowError("EMAIL JEST PUSTY");
                return false;
            }

            if (!registerWEmail.Text.Contains("@"))
            {
                ShowError("EMAIL JEST NIEPRAWIDŁOWY");
                return false;
            }

            if (string.IsNullOrWhiteSpace(registerWPassword.Text))
            {
                ShowError("HASŁO JEST PUSTE");
                return false;
            }

            if (string.IsNullOrWhiteSpace(registerWPassword2.Text))
            {
                ShowError("POWTÓRZONE HASŁO JEST PUSTE");
                return false;
            }

            if (registerWPassword.Text != registerWPassword2.Text)
            {
                ShowError("HASŁA NIE SĄ JEDNAKOWE");
                return false;
            }

            if (registerWCheck.IsChecked == false)
            {
                ShowError("BRAK ZGODY");
                return false;
            }

            return true;
        }

        private void registerWRegisterButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }


            using (var db = new SqliteDbContext())
            {
                try
                {
                    var existingUser = db.Users.FirstOrDefault(user => user.Login == registerWLogin.Text);
                    if (existingUser != null)
                    {
                        ShowError("LOGIN ZAJĘTY");
                        return;
                    }

                    var newUser = new User()
                    {
                        Login = registerWLogin.Text,
                        Haslo = registerWPassword.Text,
                        Email = registerWEmail.Text
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    LoginWindow loginWindow = new LoginWindow();
                    this.Visibility = Visibility.Hidden;
                    loginWindow.Show();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Błąd dodawania użytkownika: " + ex.Message);
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        private void registerWBack_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Visibility = Visibility.Hidden;
            mainWindow.Show();
            this.Close();
        }
    }
}
