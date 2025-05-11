using System.Windows.Controls;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    public partial class UserPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;

        public UserPage(Frame mainFrame, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            WelcomeTextBlock.Text = $"Добро пожаловать, {_user.Username}!";
        }
    }
}
