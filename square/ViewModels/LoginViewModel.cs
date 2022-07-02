using System.ComponentModel.DataAnnotations;

namespace Square.ViewModels
{
    public class LoginViewModel
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }
    }
}
