using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.ItemCategory;

public class CreateItemCategoryDto
{
	[Required]
	public string Name { get; set; }
}
