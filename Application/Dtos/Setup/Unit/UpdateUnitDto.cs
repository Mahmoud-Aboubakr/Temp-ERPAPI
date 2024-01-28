
namespace Application.Dtos.Unit
{
    public class UpdateUnitDto
    {
        public int Id { get; set; }
        public string UnitCode { get; set; }
        public string UnitNameEn { get; set; }
        public string UnitNameAr { get; set; }
        public string UnitDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
