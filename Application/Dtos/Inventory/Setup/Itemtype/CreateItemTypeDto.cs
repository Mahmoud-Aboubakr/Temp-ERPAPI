using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.Itemtype;

public class CreateItemTypeDto
{
	[Required]
	public string TypeName { get; set; }
}
