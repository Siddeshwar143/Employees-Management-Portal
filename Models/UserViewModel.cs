using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MVCDHProject.Models
***REMOVED***
    [Keyless]
    public class UserViewModel
    ***REMOVED***
        [Required]
        public string? Name ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [DataType(DataType.Password)]
        public string? Password ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Confirm Password Should match with Password.")]
        public string? ConfromPassword ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [EmailAddress]
        [Display(Name ="Email Id")]
        public string? Email ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [RegularExpression("[6-9]\\d***REMOVED***9***REMOVED***",ErrorMessage ="Moblie No. is Invalid")]
        public string? Mobile ***REMOVED*** get; set; ***REMOVED***
  ***REMOVED***
***REMOVED***
