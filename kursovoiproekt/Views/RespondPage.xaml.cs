using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kursovoiproekt.Models;

namespace kursovoiproekt.Views
{
    public partial class RespondPage : Page
    {
        private readonly JobAd _ad;
        private readonly User _currentUser;

        public RespondPage(JobAd ad, User currentUser)
        {
            InitializeComponent();
            _ad = ad;
            _currentUser = currentUser;
            DataContext = this;
        }

        public string AdTitle => $"Отклик на объявление: {_ad.Title}";

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                MessageBox.Show("Введите сообщение");
                return;
            }

            using var db = new LaborExchangeContext();
            var response = new Response
            {
                JobAdId = _ad.Id,
                ResponderId = _currentUser.Id,
                Message = MessageTextBox.Text
            };

            db.Responses.Add(response);
            db.SaveChanges();

            MessageBox.Show("Ваш отклик отправлен!");
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}


namespace kursovoiproekt.Tests
{
    [TestFixture]
    public class RespondPageTests
    {
        private LaborExchangeContext _context;
        private RespondPage _page;
        private JobAd _testAd;
        private User _testUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LaborExchangeContext>()
                .UseInMemoryDatabase(databaseName: "TestLaborExchangeDb_Respond")
                .Options;

            _context = new LaborExchangeContext(options);

            _context.Responses.RemoveRange(_context.Responses);
            _context.JobAds.RemoveRange(_context.JobAds);
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            _testAd = new JobAd { Id = 1, Title = "Test Job" };
            _testUser = new User { Id = 1, Name = "Test User" };

            _context.JobAds.Add(_testAd);
            _context.Users.Add(_testUser);
            _context.SaveChanges();

            _page = new RespondPage(_testAd, _testUser);

            var textBox = new TextBox();
            var field = typeof(RespondPage).GetField("MessageTextBox", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(_page, textBox);

            var navService = new FakeNavigationService();
            var navField = typeof(Page).GetProperty("NavigationService", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            navField.SetValue(_page, navService);
        }

        [Test]
        public void Send_Click_EmptyMessage_ShowsWarning()
        {
            var msgBoxShown = false;
            MessageBoxEventHandler handler = (s, e) => { msgBoxShown = true; };

        

            _page.MessageTextBox.Text = ""; 

          

            _page.Send_Click(null, null);

            Assert.AreEqual(0, _context.Responses.Count());
        }

        [Test]
        public void Send_Click_ValidMessage_AddsResponseAndNavigatesBack()
        {
            _page.MessageTextBox.Text = "Hello, I am interested";

            var navService = new FakeNavigationService();
            typeof(Page).GetProperty("NavigationService", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                .SetValue(_page, navService);

            _page.Send_Click(null, null);

            var response = _context.Responses.FirstOrDefault(r => r.JobAdId == _testAd.Id && r.ResponderId == _testUser.Id);
            Assert.IsNotNull(response);
            Assert.AreEqual("Hello, I am interested", response.Message);

            Assert.IsTrue(navService.GoBackCalled);
        }

        private class FakeNavigationService : System.Windows.Navigation.NavigationService
        {
            public bool GoBackCalled { get; private set; } = false;

            public override void GoBack()
            {
                GoBackCalled = true;
            }
        }
    }
}
