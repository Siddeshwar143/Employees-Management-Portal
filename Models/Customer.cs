using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCDHProject.Models
***REMOVED***
    public class Customer
    ***REMOVED***
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Custid ***REMOVED*** get; set; ***REMOVED***
        
        [MaxLength(50)]
        [Column(TypeName ="Varchar")]
        public string? Name ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName ="Money")]
        public decimal? Balance ***REMOVED*** get; set; ***REMOVED***

        [MaxLength(50)]
        [Column(TypeName = "Varchar")]
        public string? City ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(max)")]
        public string? photo ***REMOVED*** get; set; ***REMOVED***
        public bool Status ***REMOVED*** get; set; ***REMOVED***
  ***REMOVED***
***REMOVED***
