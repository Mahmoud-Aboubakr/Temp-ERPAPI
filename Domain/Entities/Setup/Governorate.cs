using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Entities.Setup;

public class Governorate : BaseEntity
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

	public int? CountryId { get; set; }
	public Country Country { get; set; }
	public List<City> Cities { get; set; } = new();
}
