using System.ComponentModel.DataAnnotations;

namespace MVCDHProject.Models
***REMOVED***
    public class ResetPasswordViewModel
    ***REMOVED***
        [Required]
        public string UserId ***REMOVED*** get; set; ***REMOVED***

        [Required]
        public string Token ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [DataType(DataType.Password)]
        public string Password ***REMOVED*** get; set; ***REMOVED***
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password should match with password.")]
        public string ConfirmPassword ***REMOVED*** get; set; ***REMOVED***

  ***REMOVED***
***REMOVED***
