using Microsoft.AspNetCore.Identity;

namespace Square.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public int Age { get; set; }
        public bool Status { get; set; }
    }
}
