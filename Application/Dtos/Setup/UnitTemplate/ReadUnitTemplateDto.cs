using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Setup.UnitTemplate
{
    public class ReadUnitTemplateDto
    {
        public int Id { get; set; }
        public string UnitTemplateCode { get; set; }
        public string UnitTemplateNameEN { get; set; }
        public string UnitTemplateNameAr { get; set; }
        public int UnitId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
