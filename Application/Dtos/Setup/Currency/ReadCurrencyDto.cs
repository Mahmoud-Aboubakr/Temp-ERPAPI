using Domain.Entities.Setup;
using System.ComponentModel.DataAnnotations;

namespace Application;

public class ReadCurrencyDto
{
    public int Id { get; set; }
    public string ArabicName { get; set; }
    public string EnglishName { get; set; }
    public string Symbol { get; set; }
    public int CountryId { get; set; }
    public ReadCountryDto Country { get; set; }
    public decimal Rate { get; set; }
    public bool IsDefault { get; set; }
}
