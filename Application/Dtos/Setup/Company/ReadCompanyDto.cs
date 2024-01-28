using Domain;

namespace Application;

public class ReadCompanyDto : BaseReadSetupDto
{
	public List<ReadBranchDto> Branches { get; set; }
}
