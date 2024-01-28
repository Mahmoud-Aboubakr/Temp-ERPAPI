using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Setup.IdentitificationTypes.Responses
{
    public class IdentitificationTypeResponseDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public short Length { get; set; }
        public bool AcceptCharacter { get; set; }
        public bool Active { get; set; }
        public bool MilitaryIdentity { get; set; }
    }
}
