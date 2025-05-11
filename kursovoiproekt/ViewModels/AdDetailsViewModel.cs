using kursovoiproekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace kursovoiproekt.ViewModels
{
    public class AdDetailsViewModel : INotifyPropertyChanged
    {
        private readonly LaborExchangeContext _db = new LaborExchangeContext();

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        private string _createdAt;
        public string CreatedAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        private string _experience;
        public string Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Comment> Comments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void LoadAd(JobAd ad)
        {
            var fullAd = _db.JobAds
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == ad.Id);

            if (fullAd == null)
                throw new ArgumentException("Объявление не найдено");

            Title = fullAd.Title;
            Description = fullAd.Description;
            Author = fullAd.User?.Username ?? "Неизвестен";
            CreatedAt = fullAd.CreatedAt.ToString("g");
            Experience = fullAd.Experience.ToString();

            Comments = new ObservableCollection<Comment>(
                _db.Comments
                   .Include(c => c.Author)
                   .Where(c => c.JobAdId == fullAd.Id)
                   .OrderByDescending(c => c.CreatedAt)
                   .ToList());
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}