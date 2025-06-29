using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using Api_Store.Data;
using Api_Store.Dtos;
using Api_Store.Entity;

namespace Api_Store.Controllers
    {

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductController (DataContext _context) : ControllerBase
        {
        [HttpGet("/GetAllProduct")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct()
            {
            var products = await _context.Product.ToListAsync();
      

            return Ok(products);

            }

        [HttpPost("/CreerUnProduit")]
           public async Task<ActionResult<Product>> CreerP([FromBody]  ProductDto dto)
            {

            if (GetUserEmail() != "admin@admin.com")
                {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Seul l'administrateur peut ajouter des produits." });
                }
            var userId = GetUserId(); // 🟢 On récupère l’ID depuis le token

            var product = new Product
                {
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
                Category = dto.Category,
                Price = dto.Price,
                Quantity = dto.Quantity,
                InternalReference = dto.InternalReference,
                ShellId = dto.ShellId,
                InventoryStatus = dto.InventoryStatus,
                Rating = dto.Rating,
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                UserId = userId 
                };

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            // Retourner le ProductDto avec l'Id généré
            var createdDto = new ProductDto
                {
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Category = product.Category,
                Price = product.Price,
                Quantity = product.Quantity,
                InternalReference = product.InternalReference,
                ShellId = product.ShellId,
                InventoryStatus = product.InventoryStatus,
                Rating = product.Rating,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                
                };

            return Ok(product);
            }


        [HttpGet("/GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
            {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
                {
                return NotFound();
                }

            return Ok(product);
            }
        


        [HttpPut("/UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
            {
            if (GetUserEmail() != "admin@admin.com")
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Seul l'administrateur peut ajouter des produits." });

            var product = await _context.Product.FindAsync(id);
            if (product == null)
                return NotFound();

            try
                {
                // Mise à jour des propriétés depuis le DTO
                product.Code = dto.Code;
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Image = dto.Image;
                product.Category = dto.Category;
                product.Price = dto.Price;
                product.Quantity = dto.Quantity;
                product.InternalReference = dto.InternalReference;
                product.ShellId = dto.ShellId;
                product.InventoryStatus = dto.InventoryStatus;
                product.Rating = dto.Rating;

                // On ne touche pas à CreatedAt, mais on met à jour UpdatedAt
                product.UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                await _context.SaveChangesAsync();

                }
            catch (Exception ex)
                {

                return BadRequest(ex.Message);
                }
         
            return Ok(true);
            }

        [HttpDelete("/SupprimerUnProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
            {
            // Vérification de l'autorisation
            if (GetUserEmail() != "admin@admin.com")
                return Forbid("Seul l'administrateur peut supprimer des produits.");

            // Recherche du produit
            var product = await _context.Product.FindAsync(id);
            if (product == null)
                return NotFound($"Produit avec l'identifiant {id} introuvable.");

            // Suppression
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
            }


        #region Private Méthode
        private string? GetUserEmail()
            {
         
            return User.FindFirstValue(ClaimTypes.Email);

            }
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
