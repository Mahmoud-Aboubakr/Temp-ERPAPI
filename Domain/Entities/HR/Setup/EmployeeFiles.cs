using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.HR.Setup
{
    public class EmployeeFiles : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int HRFileId { get; set; }
        public HRFile HRFile { get; set; }
        [Required]
        public string FilePath { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ReciveDate { get; set; }
        public bool IsConfirmed { get; set; }

    }
}
