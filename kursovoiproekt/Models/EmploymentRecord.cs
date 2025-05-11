using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace kursovoiproekt.Models
{
    public class EmploymentRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int JobAdId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? HireDate { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("JobAdId")]
        public JobAd JobAd { get; set; }
    }
}