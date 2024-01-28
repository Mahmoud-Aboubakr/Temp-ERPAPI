using System.ComponentModel.DataAnnotations;

namespace Application;

public abstract class BaseReadSetupDto
{
	public int Id { get; set; }
	public string Code { get; set; }
	public string ArabicName { get; set; }
	public string EnglishName { get; set; }
	public bool IsActive { get; set; }
}
