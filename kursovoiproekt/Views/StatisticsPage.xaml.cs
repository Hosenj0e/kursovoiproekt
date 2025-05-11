using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kursovoiproekt.Views;

namespace kursovoiproekt.Views
{
    public partial class StatisticsPage : Page
    {
        private readonly LaborExchangeContext _db = new LaborExchangeContext();

        public StatisticsPage()
        {
            InitializeComponent();
            LoadYears();
        }

        private void LoadYears()
        {
            var currentYear = DateTime.Now.Year;
            YearComboBox.ItemsSource = Enumerable.Range(currentYear - 5, 6).Reverse();
            YearComboBox.SelectedItem = currentYear;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void ShowStats_Click(object sender, RoutedEventArgs e)
        {
            if (YearComboBox.SelectedItem == null || QuarterComboBox.SelectedItem == null) return;
            int year = (int)YearComboBox.SelectedItem;
            int quarter = QuarterComboBox.SelectedIndex + 1;
            var startMonth = (quarter - 1) * 3 + 1;
            var endMonth = quarter * 3;
            var stats = _db.EmploymentRecords
                .Where(er => er.HireDate != null && 
                            er.HireDate.Value.Year == year && 
                            er.HireDate.Value.Month >= startMonth && 
                            er.HireDate.Value.Month <= endMonth) 
                .AsEnumerable()
                .GroupBy(er => er.HireDate.Value.Month)  
                .Select(g => new QuarterStatsViewModel  
                {
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    HiredCount = g.Count(),
                    AvgDays = (long)Math.Round(g.Average(x => (x.HireDate.Value - x.ApplicationDate).TotalDays))
                })
                .OrderBy(s => DateTime.ParseExact(s.Month, "MMMM", CultureInfo.CurrentCulture).Month)
                .ToList();

            StatsDataGrid.ItemsSource = stats;
        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).Tag is int userId)
            {
                var user = _db.Users.Find(userId);
                if (user != null)
                {
                    var profileWindow = new UserProfileWindow(user);
                    profileWindow.Owner = Window.GetWindow(this);
                    profileWindow.ShowDialog();
                }
            }
        }
    }

    public class QuarterStatsViewModel
    {
        public string Month { get; set; }
        public int HiredCount { get; set; }
        public long AvgDays { get; set; }
    }
}