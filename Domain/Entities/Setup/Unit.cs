
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities.Setup
{
    public class Unit: BaseEntity
    {
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string UnitCode { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string UnitNameEn { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string UnitNameAr { get; set; }
        [Column(TypeName = "nvarchar(80)")]
        public string UnitDescription { get; set; }
    }
}
