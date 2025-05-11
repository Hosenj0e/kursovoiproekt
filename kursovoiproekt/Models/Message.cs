using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoiproekt.Models
{
    [Table("Messages", Schema = "kursovoi")]

    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
    }
}
