using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    /// <summary>
    /// Логика взаимодействия для RespondPage.xaml
    /// </summary>
    public partial class RespondPage : Page
    {
        private readonly JobAd _ad;
        private readonly User _currentUser;

        public RespondPage(JobAd ad, User currentUser)
        {
            InitializeComponent();
            _ad = ad;
            _currentUser = currentUser;
            DataContext = this;
        }

        public string AdTitle => $"Отклик на объявление: {_ad.Title}";

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageBox.Show("Введите сообщение");
                return;
            }

            using var db = new LaborExchangeContext();
            var response = new Response
            {
                JobAdId = _ad.Id,
                ResponderId = _currentUser.Id,
                Message = MessageTextBox.Text
            };

            db.Responses.Add(response);
            db.SaveChanges();

            MessageBox.Show("Ваш отклик отправлен!");
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
