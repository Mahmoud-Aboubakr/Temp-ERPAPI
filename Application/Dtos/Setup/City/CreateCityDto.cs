using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateCityDto : BaseCreateSetupDto
{
	public int? GovernorateId { get; set; }
}
