using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.User;

public class CreateUserDto
{
	[Required]
	[MaxLength(100)]
	public string Username { get; set; }
	public int? EmployeeId { get; set; }
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
	public int? UserTypeId { get; set; }
	[Required]
	public SecurityLevel SecurityLevel { get; set; }
	public Language? Language { get; set; }
	public bool? HasDiscount { get; set; } = false;
	public float? DiscountPercentage { get; set; }
	public double? DiscountLimit { get; set; }
}
