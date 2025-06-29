using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Api_Store.Data;
using Api_Store.Entity;
using Api_Store.Controllers;
using Api_Store.Enum;

namespace Api_Store.TestUnit
    {
    public class ProductControllerTests
        {

        private DbContextOptions<DataContext> _options;

        public ProductControllerTests()
            {
            // Utilise la base de données en mémoire pour simuler les opérations EF Core
            _options = new DbContextOptionsBuilder<DataContext>()
                            .UseInMemoryDatabase(databaseName: "TestDb")
                            .Options;
            }
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

        [Fact]
        public async Task GetAllProduct_ReturnsListOfProducts()
            {
            // Arrange
            using (var context = new DataContext(_options))
                {
                // Nettoyer la DB en mémoire pour éviter les conflits
                context.Product.RemoveRange(context.Product);
                await context.SaveChangesAsync();

                context.Product.AddRange(
                    new Product { Id = 13, Name = "Product 1", Category= "Fitness", Code="fr", Image= "black-watch.jpg", InternalReference= "REF-123-459", Description= "Test2 Test2" },
                    new Product { Id = 14, Name = "Product 2", Category = "Fitness", Code = "fr", Image = "black-watch.jpg", InternalReference = "REF-123-459", Description = "Test3 Test3" }
                );
                await context.SaveChangesAsync();

                var controller = new ProductController(context);

                // Act
                var result = await controller.GetAllProduct();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
                Assert.Equal(2, returnProducts.Count());
                }
            }
        }
    }


