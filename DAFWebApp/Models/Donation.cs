using System.ComponentModel.DataAnnotations;

namespace DAFWebApp.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }
        [Required]
        public string? ResourceName { get; set; } 

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string? DonorName { get; set; }

        public DateTime DateDonated { get; set; } = DateTime.Now;

        public string? ContactInfo { get; set; }
    }
}
