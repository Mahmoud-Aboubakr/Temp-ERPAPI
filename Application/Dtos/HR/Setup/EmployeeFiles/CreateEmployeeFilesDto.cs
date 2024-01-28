using Domain.Entities.HR.Setup;
using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateEmployeeFilesDto
{
    [Required]
    public int EmployeeId { get; set; }
    [Required]
    public int HRFileId { get; set; }
    [Required]
    public string FilePath { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime ReciveDate { get; set; }
    public bool IsConfirmed { get; set; }
}
