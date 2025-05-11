using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace kursovoiproekt.Services
{
    public class MatchingService
    {
        public List<MatchingResult> FindMatches(JobAd jobAd)
        {
            using var db = new LaborExchangeContext();

            return db.Users
                .Where(u => u.UserType == UserType.Applicant)
                .AsEnumerable()
                .Select(u => new MatchingResult
                {
                    User = u,
                    MatchScore = CalculateMatchScore(u, jobAd)
                })
                .OrderByDescending(r => r.MatchScore)
                .Take(5)
                .ToList();
        }

        private int CalculateMatchScore(User user, JobAd jobAd)
        {
            int score = 0;

            if (!string.IsNullOrEmpty(user.Skills))
            {
                var userSkills = user.Skills.Split(',');
                var requiredSkills = jobAd.RequiredSkills.Split(',');
                score += userSkills.Intersect(requiredSkills).Count() * 10;
            }

            if (user.ExperienceYears >= jobAd.MinExperienceYears)
                score += 20;

            return score;
        }
    }

    public class MatchingResult
    {
        public User User { get; set; }
        public int MatchScore { get; set; }
    }
}