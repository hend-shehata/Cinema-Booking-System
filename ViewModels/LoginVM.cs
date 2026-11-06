using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}