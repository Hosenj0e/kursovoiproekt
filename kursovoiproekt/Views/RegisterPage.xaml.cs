using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;
using kursovoiproekt.Helpers;
using kursovoiproekt.Views;

namespace kursovoiproekt.Views
{
    public partial class RegisterPage : Page
    {
        private readonly Frame _mainFrame;

        public RegisterPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            ErrorTextBlock.Text = "";
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string email = EmailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                ErrorTextBlock.Text = "Введите логин, пароль и email.";
                return;
            }

            if (password != confirmPassword)
            {
                ErrorTextBlock.Text = "Пароли не совпадают.";
                return;
            }

            if (!IsValidEmail(email))
            {
                ErrorTextBlock.Text = "Введите корректный email.";
                return;
            }

            using var db = new LaborExchangeContext();

            if (db.Users.Any(u => u.Username == username))
            {
                ErrorTextBlock.Text = "Пользователь с таким логином уже существует.";
                return;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = PasswordHelper.HashPassword(password),
                Email = email,
                Role = (int)UserRole.User 
            };

            db.Users.Add(user);
            db.SaveChanges();

            MessageBox.Show("Регистрация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            _mainFrame.Navigate(new LoginPage(_mainFrame));
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";
            _mainFrame.Navigate(new LoginPage(_mainFrame));
        }
    }
}
