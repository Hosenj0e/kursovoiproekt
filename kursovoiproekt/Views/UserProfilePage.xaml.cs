using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    public partial class UserProfilePage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _currentUser;

        public UserProfilePage(Frame mainFrame, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _currentUser = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            UsernameTextBox.Text = _currentUser.Username;
            EmailTextBox.Text = _currentUser.Email ?? string.Empty;
            RoleTextBlock.Text = _currentUser.Role.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mainFrame.CanGoBack)
                _mainFrame.GoBack();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = PasswordBox.Password.Trim();
            string newEmail = EmailTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new LaborExchangeContext();
            var userInDb = db.Users.Find(_currentUser.Id);
            if (userInDb != null)
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    userInDb.PasswordHash = newPassword;
                }

                userInDb.Email = newEmail;

                db.SaveChanges();

                _currentUser.Email = newEmail;

                MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                PasswordBox.Clear();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LoginPage(_mainFrame));
        }
    }
}
