using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovoiproekt.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int JobAdId { get; set; }
        public JobAd JobAd { get; set; }
        public int ResponderId { get; set; }
        public User Responder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public bool IsAccepted { get; set; }
    }
}
