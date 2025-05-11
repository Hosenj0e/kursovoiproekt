using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kursovoiproekt.Models
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
    public enum UserType
    {
        Employer = 0,  
        Applicant = 1  
    }
    [Table("Users", Schema = "kursovoi")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        [Column("Role")]
        public int Role { get; set; }

        [Column("Email")]
        public string? Email { get; set; }

        public bool IsAdmin { get; set; }
        public UserType UserType { get; set; } = UserType.Applicant;
        public string Skills { get; set; } = "опыт не указан";
        public int ExperienceYears { get; set; } = 1;
    }
}
