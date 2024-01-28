using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Setup;

public class City : BaseEntity
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
	public int? GovernorateId { get; set; }
	public Governorate Governorate { get; set; }
}
