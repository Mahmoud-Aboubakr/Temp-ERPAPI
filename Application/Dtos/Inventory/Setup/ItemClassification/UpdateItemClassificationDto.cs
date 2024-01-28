using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.ItemClassification;

public class UpdateItemClassificationDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
    [Required]
    public string Name { get; set; }
}
