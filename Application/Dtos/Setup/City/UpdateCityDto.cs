using System.ComponentModel.DataAnnotations;

namespace Application;

public class UpdateCityDto : BaseUpdateSetupDto
{
	public int? GovernorateId { get; set; }
}
