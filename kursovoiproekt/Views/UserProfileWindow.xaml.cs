using kursovoiproekt.Models;
using System.Windows;

namespace kursovoiproekt.Views
{
    public partial class UserProfileWindow : Window
    {
        public UserProfileWindow(User user)
        {
            InitializeComponent();
            LoadUserData(user);
        }

        private void LoadUserData(User user)
        {
            UsernameText.Text = user.Username;
            EmailText.Text = string.IsNullOrEmpty(user.Email) ? "Не указан" : user.Email;
            RoleText.Text = user.Role.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}