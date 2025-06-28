using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using WebProduct.Entity;
using WebProduct.Data;
using Microsoft.EntityFrameworkCore;

namespace WebProduct.Controllers
    {
    [ApiController]
    [Route("api/wishlist")]
    public class WishlistController(DataContext _context) : ControllerBase
        {
     

        // GET /api/wishlist => Voir la liste d’envie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItem>>> GetWishlist()
            {
            var userId = GetUserId();
            var wishlist = await _context.WishlistItem
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return Ok(wishlist);
            }
        // POST /api/wishlist => Ajouter un produit
        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistRequest request)
            {
            var userId = GetUserId();

            // Éviter les doublons
            var existing = await _context.WishlistItem
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == request.ProductId);

            if (existing != null)
                {
                return BadRequest("Ce produit est déjà dans votre liste d'envie.");
                }

            var wishlistItem = new WishlistItem
                {
                UserId = userId,
                ProductId = request.ProductId
                };

            _context.WishlistItem.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return Ok("Produit ajouté à la liste d'envie.");
            }

        // DELETE /api/wishlist/{productId} => Supprimer un produit
        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveFromWishlist(int productId)
            {
            var userId = GetUserId();

            var item = await _context.WishlistItem
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (item == null)
                return NotFound("Produit non trouvé dans votre liste d'envie.");

            _context.WishlistItem.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
            }

        private int GetUserId()
            {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                {
                throw new UnauthorizedAccessException("L'utilisateur n'est pas authentifié.");
                }

            return int.Parse(userIdClaim); 
            }
        }
    }
