using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Setup
{
	public class Nationality : BaseEntity
	{
		[Required]
		[Column(TypeName = "nvarchar(15)")]
		public int CountryCode { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(40)")]
		public string NationalityNameEn { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(40)")]
		public string NationalityNameAr { get; set; }
		//public ICollection<User> Users { get; set; }
	}
}
