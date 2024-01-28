namespace Application;

public class ReadGovernorateDto : BaseReadSetupDto
{
    public ReadCountryDto Country { get; set; }
    public List<ReadCityDto> Cities { get; set; }
}
