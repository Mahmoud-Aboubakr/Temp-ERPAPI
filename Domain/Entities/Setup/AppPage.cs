using Domain.Entities.LookUps;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Setup
{
    public class AppPage : BaseEntity
    {
        [Required]
        public int ApplicationTblId { get; set; }
        public ApplicationTbl ApplicationTbl { get; set; }
        [Required]
        public int AppModuleId { get; set; }
        public AppModule AppModule { get; set; }
        [Required]
        public PageType PageType { get; set; }
        [Required]
        [StringLength(40)]
        public string PageNameEn { get; set; }
        [Required]
        [StringLength(40)]
        public string PageNameAr { get; set; }
        [Required]
        [StringLength(80)]
        public string PageUrl { get; set; }
        public int? Sort { get; set; }
        [Required]
        [StringLength(100)]
        public string PageDesCription { get; set; }
    }
}
