using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Shared;
using Shop.Server.Data;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
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

