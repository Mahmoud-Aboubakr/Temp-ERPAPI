using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.Itemtype;

public class UpdateItemTypeDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string TypeName { get; set; }
}
