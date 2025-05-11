using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    public partial class UserAdsPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _currentUser;
        private readonly ObservableCollection<JobAd> _ads;

        public UserAdsPage(Frame mainFrame, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _currentUser = user;
            _ads = new ObservableCollection<JobAd>();
            AdsDataGrid.ItemsSource = _ads; 
            LoadUserAds();
        }

        private void LoadUserAds()
        {
            try
            {
                using var db = new LaborExchangeContext();
                var ads = db.JobAds
                    .Where(ad => ad.UserId == _currentUser.Id)
                    .OrderByDescending(ad => ad.CreatedAt)
                    .ToList();

                _ads.Clear();
                foreach (var ad in ads)
                {
                    _ads.Add(ad);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке объявлений: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditWindow = new AddEditAdWindow(_currentUser);
            addEditWindow.Owner = Window.GetWindow(this);
            if (addEditWindow.ShowDialog() == true)
            {
                LoadUserAds();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
            else
            {
                _mainFrame.Navigate(new MainMenuPage(_mainFrame, _currentUser));
            }
        }
        private void EditAdButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdsDataGrid.SelectedItem is JobAd selectedAd)
            {
                var editWindow = new AddEditAdWindow(_currentUser, selectedAd);
                editWindow.Owner = Window.GetWindow(this);
                if (editWindow.ShowDialog() == true)
                {
                    LoadUserAds();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объявление для редактирования",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteAdButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdsDataGrid.SelectedItem is JobAd selectedAd)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить это объявление?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var db = new LaborExchangeContext();
                        var adToDelete = db.JobAds.Find(selectedAd.Id);
                        if (adToDelete != null)
                        {
                            db.JobAds.Remove(adToDelete);
                            db.SaveChanges();
                            LoadUserAds();
                            MessageBox.Show("Объявление успешно удалено",
                                "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении объявления: {ex.Message}",
                            "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объявление для удаления",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ViewResponses_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).Tag is int adId)
            {
                NavigationService.Navigate(new ResponsesPage(adId, _currentUser));
            }
        }
    }
}