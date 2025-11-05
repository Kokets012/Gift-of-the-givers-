using System.ComponentModel.DataAnnotations;

namespace DAFWebApp.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; } // for login
        public string? ContactInfo { get; set; }
        public ICollection<VolunteerTask> VolunteerTasks { get; set; } = new List<VolunteerTask>();
    }
}
