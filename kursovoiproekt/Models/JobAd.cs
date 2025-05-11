using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace kursovoiproekt.Models
{
    public enum AdType
    {
        LookingForJob = 0,
        LookingForEmployee = 1
    }

    [Table("job_ads", Schema = "kursovoi")]
    public class JobAd : INotifyPropertyChanged
    {
        private bool _isApproved;

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequiredSkills { get; set; } = "опыт не указан";
        public int MinExperienceYears { get; set; }
        public string EducationRequirements { get; set; } = "Не указано";
        public bool IsApproved
        {
            get => _isApproved;
            set
            {
                if (_isApproved != value)
                {
                    _isApproved = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime CreatedAt { get; set; }
        public int Experience { get; set; }
        public AdType AdType { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICollection<Response> Responses { get; set; } = new List<Response>();
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
    }
}
