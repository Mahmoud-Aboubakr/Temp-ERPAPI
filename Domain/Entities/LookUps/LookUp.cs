using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.LookUps
{
    public class LookUp : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [StringLength(50)]
        public string ArabicName { get; set; }
        [Required]
        [StringLength(50)]
        public string EnglishName { get; set; }
    }
}
