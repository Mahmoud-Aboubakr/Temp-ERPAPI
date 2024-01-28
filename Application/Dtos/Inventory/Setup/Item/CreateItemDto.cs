using Domain.Entities.Inventory;
using Domain.Entities.Setup;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Inventory.Setup.Item;

public class CreateItemDto
{
    [Required]
    public string ServiceName { get; set; }
    [Required]
    public string ItemCode { get; set; }
    public string BarCode { get; set; }
    public string EnglishName { get; set; }
    public string ArabicName { get; set; }
    public bool IsPermenant { get; set; }
    public bool IsActive { get; set; }
    public bool HasExpiredDate { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ItemCategoryId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ItemTypeId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ItemClassificationId { get; set; }
    public decimal? Wholesale { get; set; }
    [Range(1, int.MaxValue)]
    public int? WholesaleCurrencyId { get; set; }
    public decimal? Retail { get; set; }
    [Range(1, int.MaxValue)]
    public int? RetailCurrencyId { get; set; }
    [Required]
    public decimal CostPrice { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int CostPriceCurrencyId { get; set; }
    public string ItemImage { get; set; }
    public Stocked Stocked { get; set; }
    public string Instruction { get; set; }
    public string InstructionDescription { get; set; }
    public string Notes { get; set; }
    public string NotesDescription { get; set; }
}
