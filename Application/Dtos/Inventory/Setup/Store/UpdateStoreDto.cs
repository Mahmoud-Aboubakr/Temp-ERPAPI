using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.Store;

public class UpdateStoreDto
{
    [Required]
    public int Id { get; set; }
    public bool Manual { get; set; }
    public bool IsActive { get; set; }
    public string StoreCode { get; set; }
    [Required]
    public string StoreNameAr { get; set; }
    [Required]
    public string StoreNameEn { get; set; }
    [Required]
    public bool IsAllowSupplierTrans { get; set; }
    public bool IsLPO { get; set; }
    public bool IsEPO { get; set; }
    public bool IsGRN { get; set; }
    public bool IsIToD { get; set; }
    public bool ISScrapStore { get; set; }
    public int MainStoreId { get; set; }
    public int DepartmentId { get; set; }
    [Required]
    public string AppUserId { get; set; }
    [Required]
    public int BranchId { get; set; }
}
