using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.ItemCategory;

public class UpdateItemCategoryDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}
