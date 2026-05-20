namespace FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Item
{
    [Key]
    public int ItemId { get; set; }

    public string ItemName { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public string? ImageUrl { get; set; }

    public string? Category { get; set; }
}