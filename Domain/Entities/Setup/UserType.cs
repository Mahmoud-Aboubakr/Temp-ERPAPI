using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Setup
{
	public class UserType : BaseEntity
	{
		[Required]
		[Column(TypeName = "nvarchar(500)")]
		public string TypeName { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(500)")]
		public string DescNameAr { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(500)")]
		public string DescNameEn { get; set; }
		[Column(TypeName = "nvarchar(max)")]
		public string FullDesc { get; set; }
		//public ICollection<User> Users { get; set; }
	}
}
