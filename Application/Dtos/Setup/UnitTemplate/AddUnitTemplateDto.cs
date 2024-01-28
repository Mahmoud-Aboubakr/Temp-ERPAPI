namespace Application.Dtos.UnitTemplate
{
    public class AddUnitTemplateDto
    {
        public string UnitTemplateCode { get; set; }
        public string UnitTemplateNameEN { get; set; }
        public string UnitTemplateNameAr { get; set; }
        public int UnitId { get; set; }
        public bool IsActive { get; set; }
    }
}
