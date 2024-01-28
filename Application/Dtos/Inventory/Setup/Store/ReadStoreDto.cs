using Application.Dtos.Inventory.Setup.ItemCategory;
using Application.Dtos.Inventory.Setup.ItemClassification;
using Application.Dtos.Inventory.Setup.Itemtype;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.Store;

public class ReadStoreDto
{
	public int Id { get; set; }
    public bool Manual { get; set; }
    public bool IsActive { get; set; }
    public string StoreCode { get; set; }
    public string StoreNameAr { get; set; }
    public string StoreNameEn { get; set; }
    public bool IsAllowSupplierTrans { get; set; }

    public bool IsLPO { get; set; }
    public bool IsEPO { get; set; }
    public bool IsGRN { get; set; }
    public bool IsIToD { get; set; }
    public bool ISScrapStore { get; set; }
    public int MainStoreId { get; set; }
    public int DepartmentId { get; set; }
    public string AppUserId { get; set; }
    public int BranchId { get; set; }
}
