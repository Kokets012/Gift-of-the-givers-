using System.ComponentModel.DataAnnotations;

namespace DAFWebApp.Models
{
    public class Incident
    {
        public int IncidentId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; } 

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; } 

        [Required(ErrorMessage = "Date of incident is required")]
        public DateTime DateReported { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
