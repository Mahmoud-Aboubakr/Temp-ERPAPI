using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateGovernorateDto : BaseCreateSetupDto
{
    public int? CountryId { get; set; }
}
