using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Page
{
    public class AddAppPageDto
    {
        public int ApplicationTblId { get; set; }
        public int AppModuleId { get; set; }
        public PageType PageType { get; set; }
        public string PageNameEn { get; set; }
        public string PageNameAr { get; set; }
        public string PageUrl { get; set; }
        public int? Sort { get; set; }
        public string PageDesCription { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
