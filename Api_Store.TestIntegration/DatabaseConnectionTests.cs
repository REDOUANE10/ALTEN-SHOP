using Api_Store.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Api_Store.TestIntegration
    {
    public class DatabaseConnectionTests
        {
        private readonly IConfiguration _configuration;

        public DatabaseConnectionTests()
            {
            // Charge appsettings.json depuis le projet principal
            _configuration = new ConfigurationBuilder()
     .AddJsonFile("appsettings.Development.json")
     .Build();

            }
        [Fact]
        public async Task CanConnectToDatabase()
            {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            using var context = new DataContext(optionsBuilder.Options);

            var canConnect = await context.Database.CanConnectAsync();

            Assert.True(canConnect, "Échec de la connexion à la base de données !");
            }

        }
    }
