
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ApplicationPagePrefix
{
    public class UpdateApplicationPagePrefixDto
    {
        public int Id { get; set; }
        [Required]
        public string PageName { get; set; }
        [Required]
        public string Prefix { get; set; }
        [Required]
        public int Length { get; set; }
        public bool IsActive { get; set; }
    }
}
