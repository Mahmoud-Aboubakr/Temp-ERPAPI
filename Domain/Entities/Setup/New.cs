using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Setup
{
    public class New : BaseEntity
    {
        [Required]
        [Column(TypeName = "date")]
        public DateTime ActivateFrom { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime ActivateTo { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string NewsTextAr { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string NewsTextEn { get; set; }
    }
}
