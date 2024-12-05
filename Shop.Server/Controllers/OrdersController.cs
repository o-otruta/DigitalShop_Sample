using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Shared;
using Shop.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LiqPayClient _liqPayClient;

    public OrdersController(AppDbContext context, LiqPayClient liqPayClient)
    {
        _context = context;
        _liqPayClient = liqPayClient;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.id == order.product_id);

        if (product == null)
        {
            return NotFound(new { Message = "Product not found" });
        }

        if (product.quantity < order.quantity)
        {
            return BadRequest(new { Message = "Not enough stock available" });
        }

        product.quantity -= order.quantity;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.id }, order);
    }

    [HttpPost("liqpay-payment")]
    [Authorize]
    public async Task<IActionResult> GenerateLiqPayPayment([FromBody] Order order)
    {
        var product = await _context.Products.FindAsync(order.product_id);
        if (product == null || product.quantity < order.quantity)
        {
            return BadRequest(new { Message = "Invalid order or insufficient stock" });
        }

        order.created_at = DateTime.UtcNow;
        order.buyer_name = User.Identity?.Name;
        order.seller_name = product.created_by;
        order.product_name = product.name;
        order.product_game = product.game;
        order.product_category = product.category;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();


        var paymentUrl = _liqPayClient.GeneratePaymentUrl(
            amount: order.price * order.quantity,
            currency: "USD",
            description: $"Purchase of {order.product_name} x{order.quantity}",
            orderId: order.id.ToString()
        );

        return Ok(paymentUrl);
    }

    [HttpPost("liqpay-callback")]
    public async Task<IActionResult> LiqPayCallback([FromBody] object callbackData)
    {
        Console.WriteLine("LiqPay Callback Test:");
        Console.WriteLine(callbackData);

        // Develop create order after callback

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet("my-purchases")]
    [Authorize]
    public async Task<IActionResult> GetUserPurchases()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { Message = "User is not authenticated" });
        }

        var purchases = await _context.Orders
            .Where(o => o.buyer_name == username)
            .OrderByDescending(o => o.created_at)
            .ToListAsync();

        return Ok(purchases);
    }

    [HttpGet("my-sales")]
    [Authorize]
    public async Task<IActionResult> GetUserSales()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { Message = "User is not authenticated" });
        }

        var sales = await _context.Orders
            .Where(o => o.seller_name == username)
            .OrderByDescending(o => o.created_at)
            .ToListAsync();

        return Ok(sales);
    }
}

