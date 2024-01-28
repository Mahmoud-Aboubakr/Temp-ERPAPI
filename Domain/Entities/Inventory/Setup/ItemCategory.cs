using Domain.Entities.Setup;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory.Setup;

public class ItemCategory : BaseEntity
{
    [Required]
    [StringLength(500)]
    public string Name { get; set; }
    public List<Item> Items { get; set; } = new();
}
