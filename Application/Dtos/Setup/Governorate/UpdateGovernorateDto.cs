using System.ComponentModel.DataAnnotations;

namespace Application;

public class UpdateGovernorateDto : BaseUpdateSetupDto
{
    public int? CountryId { get; set; }
}
