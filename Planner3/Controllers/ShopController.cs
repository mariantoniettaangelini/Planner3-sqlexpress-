using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planner3.Data.DATACONTEXT;
using Planner3.Data.MODELS;
using System.Security.Claims;

namespace Planner3.Controllers
{
    [Route("api/shop")]
    [ApiController]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly PlannerContext _ctx;

        public ShopController(PlannerContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _ctx.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] Order order)
        {
            var product = await _ctx.Products.FindAsync(order.Product.Id);
            order.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (product == null)
            {
                return NotFound("Prodotto non trovato");
            }

            order.Product = product;
            order.IsCompleted = false;

            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();

            return Ok("Prodotto aggiunto");
        }

        [HttpGet("cart/{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cartItems = await _ctx.Orders
                .Where(o => o.UserId == userId && !o.IsCompleted)
                .Include(o => o.Product)
                .ToListAsync();
            if(cartItems == null || cartItems.Count == 0)
            {
                return Ok(new List<Order>()); //carrello vuoto
            }
            return Ok(cartItems);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart([FromBody] Order updatedOrder)
        {
            var order = await _ctx.Orders.FindAsync(updatedOrder.Id);

            if(order == null)
            {
                return NotFound("Ordine non trovato");
            }

            order.Quantity = updatedOrder.Quantity;
            await _ctx.SaveChangesAsync();
            return Ok("Ordine aggiornato");
        }

        [HttpDelete("remove/{orderId}")]
        public async Task<IActionResult> RemoveFromCart(int orderId)
        {
            var order = await _ctx.Orders.FindAsync(orderId);

                if(order == null)
            {
                return NotFound("Prodotto non trovato nel carrello");
            }
            _ctx.Orders.Remove(order);
            await _ctx.SaveChangesAsync();
            return Ok("Rimosso con successo");
        }

        [HttpPost("checkout{userId}")]
        public async Task<IActionResult> Checkout(int userId)
        {
            var cartItems = await _ctx.Orders
                .Where(o => o.UserId == userId && !o.IsCompleted)
                .ToListAsync();

            if(cartItems == null || cartItems.Count == 0)
            {
                return BadRequest("Carrello vuoto");
            }

            foreach(var item in cartItems)
            {
                item.IsCompleted = true;
            }
            await _ctx.SaveChangesAsync();

            return Ok("Ordine completato");
        }
    }
}
