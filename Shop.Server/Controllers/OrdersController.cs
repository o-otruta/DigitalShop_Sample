/// \file OrdersController.cs
/// \brief Controller for processing order-related requests.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Shared;
using Shop.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;

/// \brief Controller for handling order-related API endpoints.
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly LiqPayClient _liqPayClient;

    /// \brief Constructor for OrdersController.
    /// \param context Database context.
    /// \param liqPayClient LiqPay payment client.
    public OrdersController(AppDbContext context, LiqPayClient liqPayClient)
    {
        _context = context;
        _liqPayClient = liqPayClient;
    }

    /// \brief Creates a new order.
    /// \param order Order object received from the client.
    /// \return Created order or error response if product is not available.
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

    /// \brief Generates a LiqPay payment URL for the order.
    /// \param order Order object containing payment details.
    /// \return URL string for the payment or error response.
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

    /// \brief Receives callback from LiqPay after payment.
    /// \param callbackData Raw callback data from LiqPay.
    /// \return OK response.
    [HttpPost("liqpay-callback")]
    public async Task<IActionResult> LiqPayCallback([FromBody] object callbackData)
    {
        Console.WriteLine("LiqPay Callback Test:");
        Console.WriteLine(callbackData);

        // Develop create order after callback

        return Ok();
    }

    /// \brief Retrieves an order by its ID.
    /// \param id Order ID.
    /// \return Order object or NotFound if not found.
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

    /// \brief Retrieves a list of purchases for the currently authenticated user.
    /// \return List of orders made by the user.
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

    /// \brief Retrieves a list of sales made by the currently authenticated user.
    /// \return List of orders sold by the user.
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

