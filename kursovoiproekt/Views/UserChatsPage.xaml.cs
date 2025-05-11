using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace kursovoiproekt.Views
{
    public partial class UserChatsPage : Page
    {
        private readonly Frame _navigationFrame;
        private readonly User _currentUser;
        private readonly LaborExchangeContext _db;

        public UserChatsPage(Frame navigationFrame, User currentUser)
        {
            InitializeComponent();
            _navigationFrame = navigationFrame;
            _currentUser = currentUser;
            _db = new LaborExchangeContext();
        }

        private async void UserChatsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UserChatsPage_Loaded;
            await LoadChatsAsync();
        }

        private async Task LoadChatsAsync()
        {
            try
            {
                var chats = await _db.Chats
                    .Include(c => c.JobAd)
                    .Include(c => c.Messages)
                        .ThenInclude(m => m.Sender)
                    .Include(c => c.Employer)
                    .Include(c => c.Applicant)
                    .Where(c => c.EmployerId == _currentUser.Id || c.ApplicantId == _currentUser.Id)
                    .Select(c => new
                    {
                        Chat = c,
                        JobAdTitle = c.JobAd.Title ?? "Без названия",
                        OtherUser = c.EmployerId == _currentUser.Id ? c.Applicant : c.Employer,
                        LastMessage = c.Messages.OrderByDescending(m => m.SentAt).FirstOrDefault(),
                        UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != _currentUser.Id)
                    })
                    .OrderByDescending(x => x.LastMessage != null ? x.LastMessage.SentAt : DateTime.MinValue)
                    .ToListAsync();

                ChatsDataGrid.ItemsSource = chats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки чатов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChatsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ChatsDataGrid.SelectedItem == null) return;

            dynamic selected = ChatsDataGrid.SelectedItem;
            var chat = (Chat)selected.Chat;

            _navigationFrame.Navigate(new ChatPage(
                chat.JobAdId,
                chat.EmployerId,
                chat.ApplicantId,
                _currentUser));
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadChatsAsync();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_navigationFrame.CanGoBack)
            {
                _navigationFrame.GoBack();
            }
            else
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new MainMenuPage(mainWindow.MainFrame, _currentUser));
            }
        }
    }
}