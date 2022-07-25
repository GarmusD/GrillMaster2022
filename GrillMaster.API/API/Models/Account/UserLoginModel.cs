using System.ComponentModel.DataAnnotations;

namespace GrillMaster.API.Models.Account
{
    public class UserLoginModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
