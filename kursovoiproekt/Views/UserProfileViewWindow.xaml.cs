using kursovoiproekt.Models;
using System.Windows;

namespace kursovoiproekt.Views
{
    public partial class UserProfileViewWindow : Window
    {
        private readonly User _user;

        public UserProfileViewWindow(User user)
        {
            InitializeComponent();
            _user = user;
            LoadUserData();
        }

        private void LoadUserData()
        {
            UsernameTextBlock.Text = _user.Username;
            EmailTextBlock.Text = _user.Email ?? "Не указан";
            RoleTextBlock.Text = _user.Role.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}