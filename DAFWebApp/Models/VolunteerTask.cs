using System.ComponentModel.DataAnnotations;

namespace DAFWebApp.Models
{
    public class VolunteerTask
    {
        [Key]
        public int TaskId { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
    }
}
