public class CartItem
{
    public int Id { get; set; }

    public int FoodItemId { get; set; }

    public string FoodName { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public string UserId { get; set; }
}