using Domain.Entities.Setup;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Inventory.Setup;

public class Item : BaseEntity
{
    [Required]
    public string ServiceName { get; set; }
    [Required]
    public string ItemCode { get; set; }
    public string BarCode { get; set; }
    public string EnglishName { get; set; }
    public string ArabicName { get; set; }
    public bool IsPermenant { get; set; }
    public bool HasExpiredDate { get; set; }
    [Required]
    public int ItemCategoryId { get; set; }
    public ItemCategory ItemCategory { get; set; }
    [Required]
    public int ItemTypeId { get; set; }
    public ItemType ItemType { get; set; }
    [Required]
    public int ItemClassificationId { get; set; }
    public ItemClassification ItemClassification { get; set; }
    public decimal? Wholesale { get; set; }
    public int? WholesaleCurrencyId { get; set; }
    [AllowNull]
    public CurrencySetup WholesaleCurrency { get; set; }
    public decimal? Retail { get; set; }
    public int? RetailCurrencyId { get; set; }
    [AllowNull]
    public CurrencySetup RetailCurrency { get; set; }
    [Required]
    public decimal CostPrice { get; set; }
    [Required]
    public int CostPriceCurrencyId { get; set; }
    public CurrencySetup CostPriceCurrency { get; set; }
    public string ItemImage { get; set; }
    public Stocked Stocked { get; set; }
    public string Instruction { get; set; }
    public string InstructionDescription { get; set; }
    public string Notes { get; set; }
    public string NotesDescription { get; set; }
}
