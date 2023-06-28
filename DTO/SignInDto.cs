using System.ComponentModel.DataAnnotations;

namespace EcommerceBackend.DT
{ 
    public class SignInDto 
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
