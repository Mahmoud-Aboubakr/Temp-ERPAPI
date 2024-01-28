using Application.Dtos.Inventory.Setup.ItemCategory;
using Application.Dtos.Inventory.Setup.ItemClassification;
using Application.Dtos.Inventory.Setup.Itemtype;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Inventory.Setup.Item;

public class ReadItemDto
{
	public int Id { get; set; }

    
    public string ServiceName { get; set; }
    
    public string ItemCode { get; set; }
    public string BarCode { get; set; }
    public string EnglishName { get; set; }
    public string ArabicName { get; set; }
    public bool IsPermenant { get; set; }
    public bool HasExpiredDate { get; set; }
    
    public int ItemCategoryId { get; set; }
    public ReadItemCategoryDto ItemCategory{ get; set; }

    public int ItemTypeId { get; set; }
    public ReadItemTypeDto ItemType { get; set; }
    public int ItemClassificationId { get; set; }
    public ReadItemClassificationDto ItemClassification{ get; set; }
    public decimal? Wholesale { get; set; }
    public int? WholesaleCurrencyId { get; set; }
    public ReadCurrencyDto WholesaleCurrency{ get; set; }
    public decimal? Retail { get; set; }
    public int? RetailCurrencyId { get; set; }
    public ReadCurrencyDto RetailCurrency { get; set; }
    public decimal CostPrice { get; set; }
    public int CostPriceCurrencyId { get; set; }
    public ReadCurrencyDto CostPriceCurrency { get; set; }
    public string ItemImage { get; set; }
    public Stocked Stocked { get; set; }
    public string Instruction { get; set; }
    public string InstructionDescription { get; set; }
    public string Notes { get; set; }
    public string NotesDescription { get; set; }
    public bool IsActive { get; set; }
}
