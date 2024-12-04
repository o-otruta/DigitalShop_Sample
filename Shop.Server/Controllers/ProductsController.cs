using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Shared;
using Shop.Server.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] string? game_key, [FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 12, [FromQuery] string? sortOrder = null)
    {
        //var query = _context.Products.AsQueryable();
        var query = _context.Products.Where(p => p.quantity > 0);

        if (!string.IsNullOrEmpty(game_key))
        {
            query = query.Where(p => p.game_key == game_key);
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.category == category);
        }

        if (sortOrder == "lowest_price")
        {
            query = query.OrderBy(p => p.price);
        }
        else if (sortOrder == "highest_price")
        {
            query = query.OrderByDescending(p => p.price);
        }
        else
        {
            query = query.OrderByDescending(p => p.created_at);
        }

        var totalItems = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResponse<Product>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Products = products
        });
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { Message = "User is not authenticated" });
        }

        product.created_by = username;
        product.created_at = DateTime.UtcNow;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducts), new { id = product.id }, product);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = "Product not found" });
        }

        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (role != "Admin" && product.created_by != username)
        {
            return Forbid();
        }

        product.name = updatedProduct.name;
        product.price = updatedProduct.price;
        product.quantity = updatedProduct.quantity;
        product.description = updatedProduct.description;
        product.image_url = updatedProduct.image_url;

        _context.Products.Update(product);

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = "Product not found" });
        }

        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (role != "Admin" && product.created_by != username)
        {
            return Forbid();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { Message = "Product not found" });
        }

        return Ok(product);
    }

    [HttpGet("my-products")]
    public async Task<IActionResult> GetMyProducts()
    {
        var username = User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(new { Message = "User is not authenticated" });
        }

        var products = await _context.Products
                                      .Where(p => p.created_by == username)
                                      .ToListAsync();

        return Ok(products);
    }

    [HttpGet("available-games")]
    public async Task<IActionResult> GetGames()
    {
        var games = await _context.Products
            .Select(p => new { p.game, p.game_key })
            .Distinct()
            .OrderBy(g => g.game)
            .ToListAsync();

        return Ok(games);
    }

    [HttpGet("available-categories")]
    public async Task<IActionResult> GetCategories([FromQuery] string? game_key)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(game_key))
        {
            query = query.Where(p => p.game_key == game_key);
        }

        var categories = await query
            .Select(p => p.category)
            .Distinct()
            .OrderBy(category => category)
            .ToListAsync();

        return Ok(categories);
    }

    [HttpGet("game-name/{gameKey}")]
    public async Task<IActionResult> GetGameName(string gameKey)
    {
        var gameName = await _context.Products
            .Where(p => p.game_key == gameKey)
            .Select(p => p.game)
            .FirstOrDefaultAsync();

        if (gameName == null)
        {
            return NotFound("Game not found");
        }

        return Ok(gameName);
    }

}
