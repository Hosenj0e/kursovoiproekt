using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace kursovoiproekt.Views
{
    public partial class MainMenuPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _currentUser;

        public MainMenuPage(Frame mainFrame, User currentUser)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _currentUser = currentUser;

            if (_currentUser.IsAdmin)
            {
                AdminPanelButton.Visibility = Visibility.Visible;
            }

            Loaded += MainMenuPage_Loaded;
        }

        private void MainMenuPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainMenuPage_Loaded;
            LoadAds();
        }

        private void LoadAds(int adTypeFilter = -1)
        {
            try
            {
                using var db = new LaborExchangeContext();

                if (!db.Database.CanConnect())
                {
                    MessageBox.Show("Невозможно подключиться к базе данных",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                    return;
                }

                var query = db.JobAds
                    .Include(a => a.User)
                    .AsNoTracking()
                    .Where(a => a.IsApproved && a.User != null); 

                if (adTypeFilter >= 0)
                    query = query.Where(a => (int)a.AdType == adTypeFilter);

                var ads = query.OrderByDescending(a => a.CreatedAt).ToList();

                if (ads == null || !ads.Any())
                {
                    AdsDataGrid.ItemsSource = null;
                    return;
                }

                AdsDataGrid.ItemsSource = ads;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки объявлений: {ex.Message}\n\nПодробности: {ex.InnerException?.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
        private void MyAdsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new UserAdsPage(_mainFrame, _currentUser));
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new UserProfilePage(_mainFrame, _currentUser));
        }

        private void AdminPanelButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new AdminPanelPage(_mainFrame, _currentUser));
        }

        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditAdWindow(_currentUser)
            {
                Owner = Window.GetWindow(this)
            };

            if (addEditWindow.ShowDialog() == true)
            {
                LoadAds();
            }
        }

        private void AdsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AdsDataGrid.SelectedItem is JobAd selectedAd && selectedAd.User != null)
            {
                new AdDetailsWindow(selectedAd, _currentUser)
                {
                    Owner = Window.GetWindow(this)
                }.ShowDialog();
            }
        }

        private void AdTypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdTypeFilterComboBox.SelectedItem is ComboBoxItem selectedItem &&
                int.TryParse(selectedItem.Tag?.ToString(), out int filterValue))
            {
                LoadAds(filterValue);
            }
        }

        private void ChatsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToChatsPage();
        }

        private void MyChatsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigateToChatsPage();
        }

        private void NavigateToChatsPage()
        {
            try
            {
                _mainFrame.Navigate(new UserChatsPage(_mainFrame, _currentUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка перехода к чатам: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new StatisticsPage());
        }

        private void MatchingButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new MatchingPage());

        }
    }
}