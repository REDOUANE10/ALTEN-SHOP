using Microsoft.EntityFrameworkCore;
using Api_Store.Entity;

namespace Api_Store.Data
    {
    public class DataContext: DbContext
        {

        public DataContext(DbContextOptions options):base(options) { }
         public DbSet<Product> Product { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        public DbSet<WishlistItem> WishlistItem { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            // Relation un-à-plusieurs entre User et Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)

                .OnDelete(DeleteBehavior.Restrict); // Supprime les produits si l'utilisateur est supprimé
            modelBuilder.Entity<Product>()
    .Property(p => p.Price)
    .HasPrecision(18, 2);
            modelBuilder.Entity<CartItem>()
    .HasOne(ci => ci.User)
    .WithMany()
    .HasForeignKey(ci => ci.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            }
        }
    }
