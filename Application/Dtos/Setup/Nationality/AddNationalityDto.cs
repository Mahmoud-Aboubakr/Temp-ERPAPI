using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Nationality
{
    public class AddNationalityDto
    {
        public string CountryCode { get; set; }
        public string NationalityNameEn { get; set; }
        public string NationalityNameAr { get; set; }
    }
}
