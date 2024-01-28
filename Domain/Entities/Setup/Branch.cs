using Domain.Entities.Inventory.Setup;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Setup
{
	public class Branch : BaseEntity
	{
		[Required]
		[StringLength(20)]
		public string Code { get; set; }
		[Required]
		[StringLength(40)]
		public string ArabicName { get; set; }
		[Required]
		[StringLength(40)]
		public string EnglishName { get; set; }
		public int? CompanyId { get; set; }
		public Company Company { get; set; }
		public List<UserBranch> UserBranches { get; set; }
		public virtual ICollection<Store> Stores { get; set; }
	}
}
