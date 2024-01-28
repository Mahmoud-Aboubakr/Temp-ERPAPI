using System.ComponentModel.DataAnnotations;

namespace Application;

public class ReadEmployeeDto
{
    public int Id { get; set; }
    public string StaffCode { get; set; }
    
    public int Type { get; set; }
    public int AccessCardNumber { get; set; }
    
    public int BranchId { get; set; }
    
    
    public string FirstNameAr { get; set; }
    
    
    public string MiddleNameAr { get; set; }
    
    
    public string LastNameAr { get; set; }
    
    
    public string FirstNameEn { get; set; }
    
    
    public string MiddleNameEn { get; set; }
    
    
    public string LastNameEn { get; set; }
    public bool Research { get; set; }
    public bool POD { get; set; }
    
    public int Geneder { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    public string BirthPlace { get; set; }
    public int Union { get; set; }
    
    public int DepartmentId { get; set; }
    
    public int DivisioinsId { get; set; }
    
    public int ShirftType { get; set; }
    public int Religon { get; set; }
    
    public string Phone { get; set; }
    
    public int MaritalStatus { get; set; }
    
    public string NationalId { get; set; }
    
    public int IdentificationType { get; set; }
    public DateTime NationalIdEndDate { get; set; }
    
    public int Nationality { get; set; }
    public string AttendanceMachineCode { get; set; }
    public string SignatureImagePath { get; set; }
    public string EmpolyeeImagePath { get; set; }
    public string PersonalEmailAddress { get; set; }
    public string OfficialEmailAddress { get; set; }
    
    public int JobTitle { get; set; }
    
    public int JobDegree { get; set; }
    public int Country { get; set; }
    public int Governates { get; set; }
    public int City { get; set; }
    
    public int District { get; set; }
    
    public string EmerganceyContact { get; set; }
    public int Relation { get; set; }
    
    public string LocalAddress { get; set; }
    public string LocalAddress2 { get; set; }
    public string LocalAddress3 { get; set; }
    public string LocalAddress4 { get; set; }
    public string PermenantAddress { get; set; }
    public string PermenantAddress2 { get; set; }
    public string PermenantAddress3 { get; set; }
    public string PermenantAddress4 { get; set; }
    public string WorkPhone { get; set; }
    public string PlaceIncuranceIdentity { get; set; }
    public string PassportNumber { get; set; }
    public DateTime PasswportExpirDate { get; set; }
    public string DrivingLicenceNumber { get; set; }
    public DateTime DateOfIssuanceOfLicence { get; set; }
    public DateTime LicenceExpirationDate { get; set; }
    public string SyndicateCard { get; set; }
    public string WorkPermit { get; set; }
    public DateTime ExpiryDateOfWorkPermit { get; set; }
    public int BloodType { get; set; }
    public DateTime InssuranceDate { get; set; }
    public string InssuranceValue { get; set; }
    public string InssuranceJobTitle { get; set; }
    public string InssuranceNumber { get; set; }
    public DateTime DateOfAppointment { get; set; }
    
    public DateTime DateOfJoining { get; set; }
    public DateTime DateOfConfirmation { get; set; }
    public DateTime ProbationEndDate { get; set; }
    public int ProbationPeriod { get; set; }
    public string ProbationPeriodNumber { get; set; }
    
    public DateTime ContractStartingDate { get; set; }
    
    public DateTime ExpiryDateOfContact { get; set; }
    public string NoticePeriodTime { get; set; }
    public int NoticePeriod { get; set; }
    
    public int ContractPlace { get; set; }
    public int ContractType { get; set; }
    public bool IsHalfTime { get; set; }
    public bool Cader { get; set; }
    public bool TravlingTickets { get; set; }
    public int HoucingAllowance { get; set; }
    
    public int BasicSalary { get; set; }
    public bool SocialServiceFundSubscription { get; set; }
    
    public int SalaryPaymentMethod { get; set; }
    public string Remarks { get; set; }
}
