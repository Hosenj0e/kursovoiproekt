using System;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    public partial class AddEditAdWindow : Window
    {
        private readonly User _currentUser;
        private readonly JobAd _editingAd;
        private readonly bool _isEditMode;

        public AddEditAdWindow(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _isEditMode = false;
        }

        public AddEditAdWindow(User currentUser, JobAd editingAd)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _editingAd = editingAd;
            _isEditMode = true;
            LoadAdData();
        }

        private void LoadAdData()
        {
            if (_editingAd != null)
            {
                TitleTextBox.Text = _editingAd.Title;
                DescriptionTextBox.Text = _editingAd.Description;
                ExperienceTextBox.Text = _editingAd.Experience.ToString();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            string description = DescriptionTextBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите заголовок.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!int.TryParse(ExperienceTextBox.Text.Trim(), out int experience) || experience < 0)
            {
                MessageBox.Show("Введите корректный стаж работы (целое число 0 или больше).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!(AdTypeComboBox.SelectedItem is ComboBoxItem selectedItem))
            {
                MessageBox.Show("Выберите тип объявления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int adTypeValue = int.Parse(selectedItem.Tag.ToString());
            using var db = new LaborExchangeContext();
            if (_isEditMode)
            {
            }
            else
            {
                var newAd = new JobAd
                {
                    UserId = _currentUser.Id, 
                    Title = title, 
                    Description = description, 
                    Experience = experience, 
                    AdType = (AdType)adTypeValue, 
                    IsApproved = false, 
                    CreatedAt = DateTime.UtcNow 
                };
                db.JobAds.Add(newAd);
                db.SaveChanges();
                MessageBox.Show("Объявление успешно добавлено.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
