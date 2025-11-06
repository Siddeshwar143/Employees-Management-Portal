using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCDHProject.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Custid { get; set; }
        
        [MaxLength(50)]
        [Column(TypeName ="Varchar")]
        public string? Name { get; set; }

        [Column(TypeName ="Money")]
        public decimal? Balance { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "Varchar")]
        public string? City { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? photo { get; set; }
        public bool Status { get; set; }
    }
}
