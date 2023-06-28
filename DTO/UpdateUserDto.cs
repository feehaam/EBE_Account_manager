using System.ComponentModel.DataAnnotations;

namespace EcommerceBackend.DTO
{
    public class UpdateUserDto 
    {
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public int Age { get; set; }
        public string? NID { get; set; }
        public string? ProfilePicURL { get; set; }
    }
}
