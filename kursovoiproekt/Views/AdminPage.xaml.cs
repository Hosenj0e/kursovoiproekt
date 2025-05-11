using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;
using kursovoiproekt.Views;
using Microsoft.EntityFrameworkCore;

namespace kursovoiproekt.Views
{
    public partial class AdminPanelPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _currentUser;

        private ObservableCollection<User> _users;
        private ObservableCollection<JobAd> _ads;

        public AdminPanelPage(Frame mainFrame, User user)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _currentUser = user;

            LoadData();

            AdsDataGrid.RowEditEnding += AdsDataGrid_RowEditEnding;
        }

        private void LoadData()
        {
            using var db = new LaborExchangeContext();

            var users = db.Users.ToList();
            _users = new ObservableCollection<User>(users);
            UsersDataGrid.ItemsSource = _users;

            var ads = db.JobAds.Include(a => a.User).ToList();
            _ads = new ObservableCollection<JobAd>(ads);
            AdsDataGrid.ItemsSource = _ads;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private void AdsDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editedAd = e.Row.Item as JobAd;
                if (editedAd != null)
                {
                    using var db = new LaborExchangeContext();
                    var adInDb = db.JobAds.FirstOrDefault(ad => ad.Id == editedAd.Id);
                    if (adInDb != null)
                    {
                        adInDb.IsApproved = editedAd.IsApproved;
                        db.SaveChanges();
                    }
                }
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is User selectedUser)
            {
                if (selectedUser.Id == _currentUser.Id)
                {
                    MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.Username}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var db = new LaborExchangeContext();
                        var userToDelete = db.Users.Find(selectedUser.Id);
                        if (userToDelete != null)
                        {
                            db.Users.Remove(userToDelete);
                            db.SaveChanges();
                            LoadData();
                            MessageBox.Show("Пользователь успешно удален", "Успех", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteAdButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdsDataGrid.SelectedItem is JobAd selectedAd)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить объявление '{selectedAd.Title}'?",
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
                            LoadData();
                            MessageBox.Show("Объявление успешно удалено", "Успех", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении объявления: {ex.Message}", "Ошибка", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объявление для удаления", "Предупреждение", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RegisterUserButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new RegisterPage(_mainFrame));
        }
    }
}