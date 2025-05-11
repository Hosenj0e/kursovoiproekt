using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kursovoiproekt.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int JobAdId { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(10000)] 
        public string Text { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("JobAdId")]
        public virtual JobAd JobAd { get; set; }

        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}
