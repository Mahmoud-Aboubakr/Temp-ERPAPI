using System.ComponentModel.DataAnnotations;

namespace Application;

public abstract class BaseUpdateSetupDto
{
	[Required]
	public int Id { get; set; }
	[Required]
	public string Code { get; set; }
	[Required]
	[StringLength(40)]
	public string ArabicName { get; set; }
	[Required]
	[StringLength(40)]
	public string EnglishName { get; set; }
	public bool IsActive { get; set; }
}
