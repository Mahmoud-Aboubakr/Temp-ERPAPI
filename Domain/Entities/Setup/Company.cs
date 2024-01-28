using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Domain.Entities.Setup;

public class Company : BaseEntity
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
	public List<Branch> Branches { get; set; } = new();
}
