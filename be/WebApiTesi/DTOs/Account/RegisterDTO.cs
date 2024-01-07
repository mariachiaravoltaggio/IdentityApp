using System.ComponentModel.DataAnnotations;

namespace WebApiTesi.DTOs.Account
{
    public class RegisterDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Paasword must be at least {2} and maximun {1}")]
        public string Password { get; set; } = string.Empty;

    }
}
