using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Entities.Inventory.Setup;

namespace Domain.Entities.Setup;

public class CurrencySetup : BaseEntity
{
	[Required]
	[StringLength(40)]
	public string ArabicName { get; set; }
	[Required]
	[StringLength(40)]
	public string EnglishName { get; set; }
	[Required]
	[StringLength(10)]
	public string Symbol { get; set; }
    public decimal Rate { get; set; }
	public bool IsDefault { get; set; }
	[Required]
	public int CountryId { get; set; }
	public Country Country { get; set; }
    public List<Item> Wholesales { get; set; } = new();
    public List<Item> Retails { get; set; } = new();
    public List<Item> CostPrices { get; set; } = new();
}
