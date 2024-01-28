namespace Application.Dtos.Setup.UnitTemplate
{
    public class UpdateUnitTemplateDto
    {
        public int Id { get; set; }
        public string UnitTemplateCode { get; set; }
        public string UnitTemplateNameEN { get; set; }
        public string UnitTemplateNameAr { get; set; }
        public int UnitId { get; set; }
        public bool IsActive { get; set; }
    }
}
