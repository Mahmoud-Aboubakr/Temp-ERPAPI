using System.ComponentModel.DataAnnotations;
using Application.Dtos.Nationality;
using Application.Dtos.UserType;
using Domain.Entities.HR.Setup;
using Domain.Enums;

namespace Application.Dtos.User;

public class ReadUserDto
{
	public int Id { get; set; }
	public string Username { get; set; }
	public ReadEmployeeDto Employee { get; set; }
	public string ArabicName { get; set; }
	public string EnglishName { get; set; }
	public Gender Gender { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public ReadNationalityDto Nationality { get; set; }
	public ReadUserTypeDto UserType { get; set; }
	public SecurityLevel SecurityLevel { get; set; }
	public Language Language { get; set; }
	public bool HasDiscount { get; set; }
	public float DiscountPercentage { get; set; }
	public double DiscountLimit { get; set; }
}
