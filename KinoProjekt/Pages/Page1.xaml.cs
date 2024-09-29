using System.Windows.Controls;
using System.Windows;
using System.Windows.Navigation;
using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace KinoProjekt.Pages
{
    public partial class Page1 : Page
    {
        private int _loggedInUserId;
        private string _login;
        private string _password;
        private string _email;
        private string tempLogin;
        private string tempPassword;
        private string tempEmail;

        public Page1(Window1 window)
        {
            InitializeComponent();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Pages/bg.png"));
            this.Background = imageBrush;
            _loggedInUserId = window.getLoggedInUserId();
            loadUserData();
        }

        public void loadUserData()
        {
            using (var db = new SqliteDbContext())
            {
                try
                {
                    var userData = db.Users.FirstOrDefault(u => u.Id == _loggedInUserId);

                    if (userData != null)
                    {
                        page1UserId.Content += userData.Id.ToString();

                        _login = userData.Login;
                        _email = userData.Email;
                        _password = userData.Haslo;

                        page1Login.Text = _login;
                        page1Email.Text = _email;
                        page1Password.Text = _password;

                        tempLogin = _login;
                        tempPassword = _password;
                        tempEmail = _email;

                    }
                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void UpdateSaveButtonBackground()
        {
            if (page1Save.IsEnabled)
            {
                var bc = new BrushConverter();
                page1Save.Background = (Brush)bc.ConvertFrom("#0466C8");
            }
            else
            {
                page1Save.Background = (Brush)new BrushConverter().ConvertFrom("#FF97999C");
            }
        }

        private void page1EditLogin_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            page1Login.IsEnabled = true;
            page1Save.IsEnabled = true;
            UpdateSaveButtonBackground();
        }

        private void page1EditEmail_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            page1Email.IsEnabled = true;
            page1Save.IsEnabled = true;
            UpdateSaveButtonBackground();
        }

        private void page1EditPassword_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            page1Password.IsEnabled = true;
            page1Save.IsEnabled = true;
            UpdateSaveButtonBackground();
        }

        private void page1Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            tempLogin = page1Login.Text;
        }

        private void page1Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            tempEmail = page1Email.Text;
        }

        private void page1Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            tempPassword = page1Password.Text;
        }

        private void page1Save_Click(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tempLogin) || string.IsNullOrWhiteSpace(tempPassword) || string.IsNullOrWhiteSpace(tempEmail))
            {
                MessageBox.Show("Pola nie mogą być puste.");
                return;
            }

            using (var db = new SqliteDbContext())
            {
                var existingUser = db.Users.FirstOrDefault(u => u.Login == tempLogin && u.Id != _loggedInUserId);
                if (existingUser != null)
                {
                    MessageBox.Show("Login jest już zajęty przez innego użytkownika. Wybierz inny login.");
                    return;
                }

                var user = db.Users.FirstOrDefault(u => u.Id == _loggedInUserId);
                if (user != null)
                {
                    user.Login = tempLogin;
                    user.Email = tempEmail;
                    user.Haslo = tempPassword;
                    db.SaveChanges();

                    MessageBox.Show("Dane zostały zapisane.");
                }
                else
                {
                    MessageBox.Show("Nie znaleziono użytkownika.");
                }
            }
            page1Login.IsEnabled = false;
            page1Email.IsEnabled = false;
            page1Password.IsEnabled = false;
            page1Save.IsEnabled = false;
            UpdateSaveButtonBackground();
        }
    }
}
