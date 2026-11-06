using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.ViewModels
{
    public class VerifyOTPVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string OTP { get; set; } = string.Empty;
    }
}