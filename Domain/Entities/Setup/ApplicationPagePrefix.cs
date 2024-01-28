using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Setup
{
    public class ApplicationPagePrefix : BaseEntity
    {
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string PageName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(500)")]
        public string Prefix { get; set; }
        [Required]
        public int Length { get; set; }
    }
}
