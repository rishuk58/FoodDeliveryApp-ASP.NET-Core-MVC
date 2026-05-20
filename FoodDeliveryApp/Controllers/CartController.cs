using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var customerId = User.FindFirst("CustomerId")?.Value;

        if (string.IsNullOrEmpty(customerId))
        {
            return RedirectToAction("Login", "CustomerAccount");
        }

        var cart = _context.CartItems
            .Where(x => x.UserId == customerId)
            .ToList();

        return View(cart);
    }

    public IActionResult AddToCart(int id)
    {
        var item = _context.Items.Find(id);

        if (item == null)
            return NotFound();

        var customerId = User.FindFirst("CustomerId")?.Value;

        if (string.IsNullOrEmpty(customerId))
            return RedirectToAction("Login", "CustomerAccount");

        CartItem cart = new CartItem()
        {
            FoodItemId = item.ItemId,
            FoodName = item.ItemName ?? "No Name",
            Price = item.Price,
            Quantity = 1,
            ImageUrl = item.ImageUrl ?? "/images/default.png",
            UserId = customerId
        };

        _context.CartItems.Add(cart);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    public IActionResult Remove(int id)
    {
        var item = _context.CartItems.FirstOrDefault(x => x.Id == id);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult Checkout()
    {
        var customerId = User.FindFirst("CustomerId")?.Value;

        var cartItems = _context.CartItems
            .Where(x => x.UserId == customerId)
            .ToList();

        return View(cartItems);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder()
    {
        var customerId = User.FindFirst("CustomerId")?.Value;

        var cartItems = _context.CartItems
            .Where(x => x.UserId == customerId)
            .ToList();

        var order = new Order
        {
            CustomerId = int.Parse(customerId),
            OrderDateTime = DateTime.Now,
            Status = OrderStatus.Pending,
            Price = cartItems.Sum(x => x.Price * x.Quantity),
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in cartItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                ItemId = item.FoodItemId,
                Quantity = item.Quantity,
                UnitPrice = item.Price
            });
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);

        _context.SaveChanges();

        return RedirectToAction("MyOrders", "Customer");
    }
}