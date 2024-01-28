
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserType
{
    public class ReadUserTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string DescNameAr { get; set; }
        public string DescNameEn { get; set; }
        public string FullDesc { get; set; }
    }
}
