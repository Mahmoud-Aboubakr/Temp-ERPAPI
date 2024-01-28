using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Setup.IdentitificationTypes.Requests
{
    public class IdentitificationTypeCreationRequestDto
    {
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        [Required]
        public short Length { get; set; }
        public bool AcceptCharacter { get; set; }
        public bool Active { get; set; } 
        public bool MilitaryIdentity { get; set; }
    }
}
