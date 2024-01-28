using System.ComponentModel.DataAnnotations;

namespace Application;

public class ReadHRFileDto
{
    public int Id { get; set; }
    public string HiringDocument { get; set; }
    public bool IsMandatory { get; set; }
    public int Status { get; set; }
}
