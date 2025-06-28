using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using WebProduct.Entity;
using WebProduct.Data;
using Microsoft.EntityFrameworkCore;
using WebProduct.Dtos;

namespace WebProduct.Controllers
    {
    [ApiController]
   // [Route("api/cart")]
    public class CartController(DataContext _context) : ControllerBase
        {
       
        [HttpPost("/AjouterAuPanier")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
            {
            var userId = GetUserId();

            var existingItem = await _context.CartItem
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

            if (existingItem != null)
                {
                existingItem.Quantity += request.Quantity;
                }
            else
                {
                var cartItem = new CartItem
                    {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                    };
                _context.CartItem.Add(cartItem);
                }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Produit ajouté au panier." });
            }

        [HttpDelete("car/Supprimer/{productId}")]
        public async Task<IActionResult>  RemoveFromCart(int productId)
            {
           var userId = GetUserId();
            var item = await _context.CartItem
       .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);


            if (item == null)
                return NotFound(new { message = "Produit non trouvé dans le panier." });
            _context.CartItem.Remove(item); 

            await _context.SaveChangesAsync();

            return NoContent();
            }

        #region Private methodes
        private int GetUserId()
            {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Utilisateur non authentifié.");

            return int.Parse(userIdClaim);
            }
        #endregion






        }
    }
