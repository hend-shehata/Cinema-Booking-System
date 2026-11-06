using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.ViewModels
{
    public class ResendEmailConfirmationVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}