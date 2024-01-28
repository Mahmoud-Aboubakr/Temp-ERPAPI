﻿using System.ComponentModel.DataAnnotations;

namespace Application;

public class UpdateCurrencyDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string ArabicName { get; set; }
    [Required]
    public string EnglishName { get; set; }
    [Required]
    public string Symbol { get; set; }
    [Required]
    public int CountryId { get; set; }
    public decimal Rate { get; set; }
    public bool IsDefault { get; set; }
}
