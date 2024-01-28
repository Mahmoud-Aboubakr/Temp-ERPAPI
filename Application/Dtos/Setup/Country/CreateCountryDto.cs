using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateCountryDto : BaseCreateSetupDto
{
	public List<int> GovernorateIds { get; set; }
}
