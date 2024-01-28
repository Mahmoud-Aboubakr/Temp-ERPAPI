using System.ComponentModel.DataAnnotations;

namespace Application;

public class ReadEmployeeFilesDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int HRFileId { get; set; }
    public string FilePath { get; set; }
    public DateTime ExpiryDate { get; set; }
    public DateTime ReciveDate { get; set; }
    public bool IsConfirmed { get; set; }
}
