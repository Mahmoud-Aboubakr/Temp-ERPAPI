
using Application.Dtos.Setup.LookUps;
using Domain.Enums;

namespace Application.Dtos.Page
{
    public class ReadAppPageDto
    {
        public int Id { get; set; }
        public int ApplicationTblId { get; set; }
        public int AppModuleId { get; set; }
        public PageType PageType { get; set; }
        public string PageNameEn { get; set; }
        public string PageNameAr { get; set; }
        public string PageUrl { get; set; }
        public int? Sort { get; set; }
        public string PageDesCription { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


    }
}
