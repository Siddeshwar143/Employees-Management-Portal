using System.ComponentModel.DataAnnotations;

namespace MVCDHProject.Models
***REMOVED***
    public class LoginViewModel
    ***REMOVED***
        [Required]
        public string? Name ***REMOVED*** get; set; ***REMOVED***

        [Required]
        [DataType(DataType.Password)]
        public string? Password ***REMOVED*** get; set; ***REMOVED***

        [Display(Name ="Remember Me")]
        public bool Rememberme ***REMOVED*** get; set; ***REMOVED***
        public string ReturnUrl ***REMOVED*** get; set; ***REMOVED*** = "";
  ***REMOVED***
***REMOVED***
