using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.HR.Setup
{
    public class HRFile : BaseEntity
    {
        [Required]
        public string HiringDocument { get; set; }
        public bool IsMandatory { get; set; }
        public int Status { get; set; }
        public List<EmployeeFiles> EmployeeFiles { get; set; }
    }
}
