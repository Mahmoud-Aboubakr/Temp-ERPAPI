
namespace Application.Dtos.Unit
{
    public class ReadUnitDto
    {
        public int Id { get; set; }
        public string UnitCode { get; set; }
        public string UnitNameEn { get; set; }
        public string UnitNameAr { get; set; }
        public string UnitDescription { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
