using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore; 

namespace kursovoiproekt.Views
{
    public partial class ResponsesPage : Page
    {
        private readonly JobAd _ad;
        private readonly User _currentUser;
        private readonly LaborExchangeContext _db;

        public ResponsesPage(int adId, User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _db = new LaborExchangeContext();

            _ad = _db.JobAds
                .Include(a => a.Responses)
                .ThenInclude(r => r.Responder)
                .FirstOrDefault(a => a.Id == adId);

            DataContext = this;
            ResponsesGrid.ItemsSource = _ad?.Responses;

            this.Unloaded += ResponsesPage_Unloaded;
        }

        public string AdTitle => $"Отклики на объявление: {_ad?.Title}";

        private void Respond_Click(object sender, RoutedEventArgs e)
        {
            if (ResponsesGrid.SelectedItem is Response selectedResponse)
            {
                NavigationService.Navigate(new ChatPage(
                    jobAdId: _ad.Id,
                    employerId: _ad.UserId,
                    applicantId: selectedResponse.ResponderId,
                    currentUser: _currentUser));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ResponsesPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _db.Dispose();
        }
    }
}
