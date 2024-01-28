using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateHRFileDto
{
    [Required]
    public string HiringDocument { get; set; }
    public bool IsMandatory { get; set; }
    public int Status { get; set; }
}
