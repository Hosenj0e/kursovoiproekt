using kursovoiproekt.Models;
using kursovoiproekt.Views;
using System.Windows;

namespace kursovoiproekt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new LoginPage(MainFrame));
        }
    }
}