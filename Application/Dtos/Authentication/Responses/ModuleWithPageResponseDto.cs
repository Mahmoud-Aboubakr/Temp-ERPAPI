using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Authentication.Responses
{
    public class RolePrivilegeResponseDto
    {
        public string Parent { get; set; }
        public List<ModuleWithPageResponseDto> Modules { get; set; }
    }

    public class ModuleWithPageResponseDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<PageTypeWithPage> Types { get; set; }
    }

    public class PageTypeWithPage
    {
        public int TypeId { get; set; }
        public string PageType { get; set; }
        public List<Page> Pages { get; set; }
    }

    public class Page
    {
        public int PageId { get; set; }
        public string PageNameEn { get; set; }
        public string PageNameAr { get; set; }
        public string PageUrl { get; set; }
        public int? Sort { get; set; }
        public string PageDesCription { get; set; }
    }
}
