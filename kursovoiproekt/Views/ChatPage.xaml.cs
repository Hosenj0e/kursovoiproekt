using kursovoiproekt.Models;
using kursovoiproekt.Converters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;

namespace kursovoiproekt.Views
{

    public partial class ChatPage : Page, INotifyPropertyChanged
    {
        private readonly LaborExchangeContext _db;
        private readonly User _currentUser;
        private Chat _chat;
        private string _jobAdTitle;
        private string _interlocutorName;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<MessageViewModel> _messages = new ObservableCollection<MessageViewModel>();
        public ObservableCollection<MessageViewModel> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged(nameof(Messages));
            }
        }

        public string JobAdTitle
        {
            get => _jobAdTitle;
            set
            {
                _jobAdTitle = value;
                OnPropertyChanged(nameof(JobAdTitle));
            }
        }
        public string InterlocutorName
        {
            get => _interlocutorName;
            set
            {
                _interlocutorName = value;
                OnPropertyChanged(nameof(InterlocutorName));
            }
        }
        private DispatcherTimer _refreshTimer;

        public ChatPage(int jobAdId, int employerId, int applicantId, User currentUser)
        {
            InitializeComponent();
            _db = new LaborExchangeContext();
            _currentUser = currentUser;
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5);
            _refreshTimer.Tick += async (s, e) => await LoadMessagesAsync();
            _refreshTimer.Start(); 
            Unloaded += (s, e) => _refreshTimer.Stop();

            Loaded += async (s, e) => await InitializeChatAsync(jobAdId, employerId, applicantId);
            Unloaded += (s, e) => _db?.Dispose();
        }

        private async Task InitializeChatAsync(int jobAdId, int employerId, int applicantId)
        {
            try
            {
                _chat = await _db.Chats
                    .Include(c => c.JobAd)
                    .Include(c => c.Messages)
                        .ThenInclude(m => m.Sender)
                    .FirstOrDefaultAsync(c => c.JobAdId == jobAdId &&
                                           c.EmployerId == employerId &&
                                           c.ApplicantId == applicantId);

                if (_chat == null)
                {
                    _chat = new Chat
                    {
                        JobAdId = jobAdId,
                        EmployerId = employerId,
                        ApplicantId = applicantId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _db.Chats.Add(_chat);
                    await _db.SaveChangesAsync();

                    JobAdTitle = _chat.JobAd?.Title ?? "Новый чат";
                    await LoadMessagesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации чата: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task LoadMessagesAsync()
        {
            try
            {
                if (_chat == null || _chat.Id == 0) return;

                var messages = await _db.Messages
                    .Include(m => m.Sender) 
                    .Where(m => m.ChatId == _chat.Id)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Clear();
                    foreach (var message in messages)
                    {
                        Messages.Add(new MessageViewModel
                        {
                            Content = message.Content,
                            SentAt = message.SentAt,
                            IsMyMessage = message.Sender.Id == _currentUser.Id,
                            Sender = message.Sender 
                        });
                    }
                    OnPropertyChanged(nameof(Messages));
                    ScrollToBottom();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}\n{ex.InnerException?.Message}");
            }
        }


        private async Task MarkMessagesAsReadAsync()
        {
            try
            {
                var unreadMessages = await _db.Messages
                    .Where(m => m.ChatId == _chat.Id &&
                               !m.IsRead &&
                               m.Sender.Id != _currentUser.Id) 
                    .ToListAsync();

                if (unreadMessages.Any())
                {
                    foreach (var message in unreadMessages)
                    {
                        message.IsRead = true;
                    }
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отметке сообщений как прочитанных: {ex.Message}");
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageBox.Show("Введите текст сообщения", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_chat == null || _chat.Id == 0)
                {
                    MessageBox.Show("Чат не инициализирован", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var message = new Message
                {
                    ChatId = _chat.Id,
                    SenderId = _currentUser.Id, 
                    Content = MessageTextBox.Text.Trim(),
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _db.Messages.Add(message);
                await _db.SaveChangesAsync();

                var newMessage = await _db.Messages
                    .Include(m => m.Sender)
                    .FirstOrDefaultAsync(m => m.Id == message.Id);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add(new MessageViewModel
                    {
                        Content = newMessage.Content,
                        SentAt = newMessage.SentAt,
                        IsMyMessage = newMessage.Sender.Id == _currentUser.Id
                    });

                    MessageTextBox.Clear();
                    ScrollToBottom();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetDbErrorDetails(DbUpdateException ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);

            if (ex.InnerException != null)
            {
                sb.AppendLine($"Inner Exception: {ex.InnerException.Message}");
            }

            return sb.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
            {
                NavigationService.GoBack();
            }
            else
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow?.MainFrame.Navigate(new MainMenuPage(mainWindow.MainFrame, _currentUser));
            }
        }

        private void ScrollToBottom()
        {
            if (MessagesList.Items.Count > 0)
            {
                MessagesList.ScrollIntoView(MessagesList.Items[MessagesList.Items.Count - 1]);
            }
        }

        private async Task<Chat> CreateNewChatAsync(int jobAdId, int employerId, int applicantId)
        {
            var chat = new Chat
            {
                JobAdId = jobAdId,
                EmployerId = employerId,
                ApplicantId = applicantId,
                CreatedAt = DateTime.Now
            };

            _db.Chats.Add(chat);
            await _db.SaveChangesAsync();
            return chat;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    return;
                }
                SendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            SendButton_Click(sender, e);
        }
    }

    public class MessageViewModel
    {
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsMyMessage { get; set; }
        public User Sender { get; internal set; }
    }


}