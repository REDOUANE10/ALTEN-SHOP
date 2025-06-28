using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebProduct.Data;
using WebProduct.Entity;
using WebProduct.Controllers;
using WebProduct.Enum;

namespace WebProduct.TestUnit
    {
    public class ProductControllerTests
        {
        [Fact]
        public async Task DeleteProduct_AdminUser_ProductDeleted_ReturnsNoContent()
            {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("DeleteProductTest")
                .EnableSensitiveDataLogging() // utile pour debug si erreur
                .Options;

            using (var context = new DataContext(options))
                {
                var productId = 4;

                var product = new Product
                    {
                    Id = productId,
                    Code = "P004",
                    Name = "Test Product",
                    Description = "Description test",
                    Image = "image.jpg",
                    Category = "Catégorie test",
                    Price = 10.99m,
                    Quantity = 50,
                    InternalReference = "REF004",
                    ShellId = 1,
                    InventoryStatus = InventoryStatus.InStock,
                    Rating = 4,
                    CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UpdatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    UserId = 1, // simuler une clé étrangère valide
                    User = new User
                        {
                        Id = 1,
                        Email = "admin@admin.com",
                        Firstname = "Admin Test",
                        Username = "admin",
                        PasswordHash = new byte[] { 1, 2, 3 },  // valeur bidon
                        PasswordSalt = new byte[] { 4, 5, 6 }   // valeur bidon
                        }
                    };

                context.User.Add(product.User);
                context.Product.Add(product);
                await context.SaveChangesAsync();

                var controller = new ProductController(context);

                // Simuler un utilisateur admin
                var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, "admin@admin.com")
                }, "TestAuthentication"));

                controller.ControllerContext = new ControllerContext
                    {
                    HttpContext = new DefaultHttpContext { User = user }
                    };

                // Act
                var result = await controller.DeleteProduct(productId);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Null(await context.Product.FindAsync(productId));
                }
            }
        }
    }
