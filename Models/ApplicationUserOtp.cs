
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema_Booking_System.Models
{
    public class ApplicationUserOtp
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        public string OTP { get; set; } = string.Empty;

        public bool IsValid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ValidTo { get; set; }

        public ApplicationUserOtp()
        {
        }

        public ApplicationUserOtp(string applicationUserId, string otp)
        {
            ApplicationUserId = applicationUserId;
            OTP = otp;
            IsValid = true;
            CreatedAt = DateTime.UtcNow;
            ValidTo = DateTime.UtcNow.AddMinutes(10);
        }
    }
}
