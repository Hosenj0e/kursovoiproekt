using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace kursovoiproekt.Views
{
    public partial class AdDetailsWindow : Window
    {
        private readonly JobAd _ad;
        private readonly User _currentUser;
        private readonly LaborExchangeContext _db;

        public AdDetailsWindow(JobAd ad, User currentUser)
        {
            InitializeComponent();
            _ad = ad;
            _currentUser = currentUser;
            _db = new LaborExchangeContext();

            DataContext = _ad;
            InitializeChatButton();
            Loaded += async (s, e) => await LoadCommentsAsync();
        }

        private void InitializeChatButton()
        {
            if (_ad.UserId == _currentUser.Id)
            {
                var chatButton = FindName("OpenChatButton") as Button;
                if (chatButton != null)
                {
                    chatButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AuthorProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new UserProfileViewWindow(_ad.User);
            profileWindow.Owner = this;
            profileWindow.ShowDialog();
        }

        private async void AddComment_Click(object sender, RoutedEventArgs e)
        {
            string commentText = CommentTextBox.Text.Trim();

            if (string.IsNullOrEmpty(commentText))
            {
                MessageBox.Show("Введите текст комментария.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            if (commentText.Length > 10000)
            {
                MessageBox.Show("Комментарий слишком длинный (максимум 10000 символов).",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var jobAdExists = await _db.JobAds.AnyAsync(j => j.Id == _ad.Id);
                var authorExists = await _db.Users.AnyAsync(u => u.Id == _currentUser.Id);

                if (!jobAdExists || !authorExists)
                {
                    MessageBox.Show("Не найдено связанное объявление или пользователь.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                
                var comment = new Comment
                {
                    JobAdId = _ad.Id,
                    AuthorId = _currentUser.Id,
                    Text = commentText,
                    CreatedAt = DateTime.UtcNow,
                    JobAd = await _db.JobAds.FindAsync(_ad.Id),
                    Author = await _db.Users.FindAsync(_currentUser.Id)
                };

                _db.Comments.Add(comment);
                await _db.SaveChangesAsync();

                CommentTextBox.Clear();
                await LoadCommentsAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DbUpdateException: {dbEx}");
                Console.WriteLine($"InnerException: {dbEx.InnerException}");

                string errorMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                MessageBox.Show($"Ошибка при сохранении комментария: {errorMessage}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                MessageBox.Show($"Неожиданная ошибка: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadCommentsAsync()
        {
            try
            {
                var comments = await _db.Comments
                    .Include(c => c.Author)
                    .Where(c => c.JobAdId == _ad.Id)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                CommentsListBox.ItemsSource = comments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки комментариев: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenChat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentUser.Id == _ad.User.Id)
                {
                    MessageBox.Show("Вы не можете открыть чат с самим собой",
                        "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var chatPage = new ChatPage(_ad.Id, _ad.User.Id, _currentUser.Id, _currentUser);
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(chatPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия чата: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _db?.Dispose();
        }
    }
}