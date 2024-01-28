using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Nationality
{
    public class UpdateNationalityDto
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string NationalityNameEn { get; set; }
        public string NationalityNameAr { get; set; }
        public bool IsActive { get; set; }
    }
}
