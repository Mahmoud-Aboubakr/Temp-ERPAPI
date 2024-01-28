
namespace Application.Dtos.Nationality
{
    public class ReadNationalityDto
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string NationalityNameEn { get; set; }
        public string NationalityNameAr { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
