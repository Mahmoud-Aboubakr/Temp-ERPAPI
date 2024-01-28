using Domain.Entities.Identity;
using Domain.Entities.Setup;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Inventory.Setup;

public class Store : BaseEntity
{
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
    public AppUser AppUser { get; set; }
    public Branch Branch { get; set; }
    public bool ISScrapStore { get; set; }
    [ForeignKey("MainStoreId")]
    public virtual Store MainStore { get; set; }
    public virtual ICollection<Store> SubStores { get; set; }
    [Required]
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    [ForeignKey("AppUser")]
    [Required]
    public string AppUserId { get; set; }
    [ForeignKey("Branch")]
    [Required]
    public int BranchId { get; set; }
    public bool Manual { get; set; }
}
