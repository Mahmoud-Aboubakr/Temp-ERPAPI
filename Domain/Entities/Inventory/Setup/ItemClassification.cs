using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory.Setup;

public class ItemClassification : BaseEntity
{
    [Required]
    [StringLength(20)]
    public string Code { get; set; }
    [Required]
    [StringLength(500)]
    public string Name { get; set; }
    public List<Item> Items { get; set; } = new();
}
