using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.HR.Setup
{
    public class IdentityType: BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public short Length { get; set; }
        public bool IsAcceptingCharacter { get; set; }
        public bool HasMilitaryIdentity { get; set; }
    }
}
