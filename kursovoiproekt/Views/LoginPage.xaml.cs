using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;
using kursovoiproekt.Helpers;

namespace kursovoiproekt.Views
{
    public partial class LoginPage : Page
    {
        private readonly Frame _mainFrame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new LaborExchangeContext();
            var user = db.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _mainFrame.Navigate(new MainMenuPage(_mainFrame, user));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new RegisterPage(_mainFrame));
        }
    }
}
