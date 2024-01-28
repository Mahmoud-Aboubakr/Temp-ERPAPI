using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Entities;

namespace Domain.Entities.Setup;

public class Country : BaseEntity
{
	[Required]
	[StringLength(20)]
	public string Code { get; set; }
	[Required]
	[StringLength(40)]
	public string ArabicName { get; set; }
	[Required]
	[StringLength(40)]
	public string EnglishName { get; set; }

	public List<Governorate> Governorates { get; set; } = new();
	public List<CurrencySetup> Currencies { get; set; } = new();
}