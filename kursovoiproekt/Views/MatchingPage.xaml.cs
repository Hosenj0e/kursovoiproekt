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

    public partial class MatchingPage : Page
    {
        private readonly LaborExchangeContext _db = new LaborExchangeContext();

        public MatchingPage()
        {
            InitializeComponent();
            LoadJobAds();
        }
        private void LoadJobAds()
        {
            JobAdComboBox.ItemsSource = _db.JobAds.ToList();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void FindCandidates_Click(object sender, RoutedEventArgs e)
        {            
            if (JobAdComboBox.SelectedItem is JobAd selectedAd)
            {
                var candidates = _db.Users
                    .Where(u => u.UserType == UserType.Applicant) 
                    .ToList()
                    .Select(u => new CandidateViewModel 
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Skills = u.Skills,
                        ExperienceYears = u.ExperienceYears,
                        MatchScore = CalculateMatchScore(u, selectedAd) 
                    })
                    .OrderByDescending(x => x.MatchScore) 
                    .ToList();

                CandidatesGrid.ItemsSource = candidates; 
            }
        }

        private int CalculateMatchScore(User user, JobAd jobAd)
        {
            int score = 0;
            if (user.Skills != null && jobAd.RequiredSkills != null &&
                user.Skills.Contains(jobAd.RequiredSkills))
                score += 50;
            if (user.ExperienceYears >= jobAd.MinExperienceYears)
                score += 30;
            return score;
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

    public class CandidateViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Skills { get; set; }
        public int ExperienceYears { get; set; }
        public int MatchScore { get; set; }
    }
}

