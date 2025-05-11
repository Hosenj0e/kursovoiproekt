using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoiproekt.Models
{
    [Table("Chats", Schema = "kursovoi")]

    public class Chat
    {
        public int Id { get; set; }

        [Column("JobAdId")] 
        public int JobAdId { get; set; }

        [ForeignKey("JobAdId")]
        public JobAd JobAd { get; set; }
        public int EmployerId { get; set; }
        public User Employer { get; set; }
        public int ApplicantId { get; set; }
        public User Applicant { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User GetOtherUser(User currentUser)
        {
            return EmployerId == currentUser.Id ? Applicant : Employer;
        }
    }
}
