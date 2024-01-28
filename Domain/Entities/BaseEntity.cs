
using System.ComponentModel.DataAnnotations;


namespace Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public int? EditedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreateDate { get; set;}
        public DateTime? EditDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
