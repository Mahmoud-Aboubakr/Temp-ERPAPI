using System.ComponentModel.DataAnnotations;

namespace Application;

public class CreateCompanyDto : BaseCreateSetupDto
{
    public List<int> BranchesIds { get; set; }
}
