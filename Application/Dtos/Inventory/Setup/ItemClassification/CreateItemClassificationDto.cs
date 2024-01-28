using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.ItemClassification;

public class CreateItemClassificationDto
{
	[Required]
	public string Code { get; set; }
	[Required]
	public string Name { get; set; }
}
