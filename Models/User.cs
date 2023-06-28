using Microsoft.AspNetCore.Identity;

namespace EcommerceBackend.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? NID { get; set; }
        public string? ProfilePicURL { get; set; }
    }
}
