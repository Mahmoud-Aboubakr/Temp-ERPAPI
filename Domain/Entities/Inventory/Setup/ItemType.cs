using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Inventory.Setup;

public class ItemType : BaseEntity
{
    [Required]
    [StringLength(500)]
    public string TypeName { get; set; }
    public List<Item> Items { get; set; } = new();
}
