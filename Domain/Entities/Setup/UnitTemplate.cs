
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Setup
{
    public class UnitTemplate:BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string UnitTemplateCode { get; set; }
        [Required]
        [StringLength(50)]
        public string UnitTemplateNameEN { get; set; }
        [Required]
        [StringLength(50)]
        public string UnitTemplateNameAr { get; set; }
        public int? UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
