using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVCDHProject.Models
{
    [Keyless]
    public class UserViewModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Confirm Password Should match with Password.")]
        public string? ConfromPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Email Id")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression("[6-9]\\d{9}",ErrorMessage ="Moblie No. is Invalid")]
        public string? Mobile { get; set; }
    }
}
