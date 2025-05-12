using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using kursovoiproekt.Models;
using kursovoiproekt.Views;
using System;
using System.Linq;
using System.Windows.Controls;

namespace kursovoiproekt.Tests
{
    [TestFixture]
    public class StatisticsPageTests
    {
        private LaborExchangeContext _context;
        private StatisticsPage _page;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LaborExchangeContext>()
                .UseInMemoryDatabase(databaseName: "TestLaborExchangeDb")
                .Options;

            _context = new LaborExchangeContext(options);

            _context.EmploymentRecords.RemoveRange(_context.EmploymentRecords);
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            _context.EmploymentRecords.AddRange(
                new EmploymentRecord
                {
                    Id = 1,
                    ApplicationDate = new DateTime(2024, 1, 10),
                    HireDate = new DateTime(2024, 1, 20),
                    UserId = 1
                },
                new EmploymentRecord
                {
                    Id = 2,
                    ApplicationDate = new DateTime(2024, 2, 5),
                    HireDate = new DateTime(2024, 2, 15),
                    UserId = 2
                },
                new EmploymentRecord
                {
                    Id = 3,
                    ApplicationDate = new DateTime(2024, 4, 1),
                    HireDate = new DateTime(2024, 4, 10),
                    UserId = 3
                }
            );
            _context.SaveChanges();

            _page = new StatisticsPage();

            var dbField = typeof(StatisticsPage).GetField("_db", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            dbField.SetValue(_page, _context);

            _page.YearComboBox = new ComboBox();
            _page.QuarterComboBox = new ComboBox();

            var currentYear = 2024;
            _page.YearComboBox.ItemsSource = Enumerable.Range(currentYear - 5, 6).Reverse();
            _page.YearComboBox.SelectedItem = 2024;

            _page.QuarterComboBox.Items.Add("Q1");
            _page.QuarterComboBox.Items.Add("Q2");
            _page.QuarterComboBox.Items.Add("Q3");
            _page.QuarterComboBox.Items.Add("Q4");
        }

        [Test]
        public void ShowStats_Click_Q1_2024_ReturnsCorrectStats()
        {
            _page.QuarterComboBox.SelectedIndex = 0;

            _page.StatsDataGrid = new DataGrid();

            _page.ShowStats_Click(null, null);

            var items = _page.StatsDataGrid.ItemsSource.Cast<QuarterStatsViewModel>().ToList();

            Assert.AreEqual(2, items.Count);

            var janStats = items.FirstOrDefault(s => s.Month == "January");
            Assert.IsNotNull(janStats);
            Assert.AreEqual(1, janStats.HiredCount);
            Assert.AreEqual(10, janStats.AvgDays); 

            var febStats = items.FirstOrDefault(s => s.Month == "February");
            Assert.IsNotNull(febStats);
            Assert.AreEqual(1, febStats.HiredCount);
            Assert.AreEqual(10, febStats.AvgDays); 
        }

        [Test]
        public void ShowStats_Click_Q2_2024_ReturnsCorrectStats()
        {
            _page.QuarterComboBox.SelectedIndex = 1; 

            _page.StatsDataGrid = new DataGrid();

            _page.ShowStats_Click(null, null);

            var items = _page.StatsDataGrid.ItemsSource.Cast<QuarterStatsViewModel>().ToList();

            
            Assert.AreEqual(1, items.Count);

            var aprStats = items.FirstOrDefault(s => s.Month == "April");
            Assert.IsNotNull(aprStats);
            Assert.AreEqual(1, aprStats.HiredCount);
            Assert.AreEqual(9, aprStats.AvgDays); // 10-1 дней = 9
        }
    }
}
