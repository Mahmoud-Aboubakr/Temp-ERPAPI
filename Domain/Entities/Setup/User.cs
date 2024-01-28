using System.ComponentModel.DataAnnotations;
using Domain.Entities.HR.Setup;
using Domain.Enums;

namespace Domain.Entities.Setup;

public class User : BaseEntity
{
	[Required]
	[MaxLength(100)]
	public string Username { get; set; }
	[Required]
	public int EmployeeId { get; set; }
	public Employee Employee { get; set; }
	[Required]
	[MaxLength(200)]
	public string ArabicName { get; set; }
	[Required]
	[MaxLength(200)]
	public string EnglishName { get; set; }
	[Required]
	public Gender Gender { get; set; }
	[Required]
	public DateTime DateOfBirth { get; set; }
	[Required]
	[MaxLength(50)]
	public string PhoneNumber { get; set; }
	[Required]
	[MaxLength(75)]
	public string Email { get; set; }
	public int? NationalityId { get; set; }
	public Nationality Nationality { get; set; }
	// TODO: Waiting for Departments
	// public int? DepartmentId { get; set; }
	// public Department Department { get; set; }
	public int? UserTypeId { get; set; }
	public UserType UserType { get; set; }
	public ICollection<UserBranch> Branches { get; set; }
	[Required]
	public SecurityLevel SecurityLevel { get; set; }
	// TODO: Waiting for Password
	// public string Password { get; set; }
	// public string PasswordSalt { get; set; }
	// public int? ModuleId { get; set; }
	// public Module Module { get; set; }
	public Language? Language { get; set; }
	public bool HasDiscount { get; set; }
	public float DiscountPercentage { get; set; }
	public double DiscountLimit { get; set; }

	// TODO: Waiting for UserRoles
	// public List<UserRole> UserRoles { get; set; }
}
