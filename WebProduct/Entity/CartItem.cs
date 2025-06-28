namespace WebProduct.Entity
    {
    public class CartItem
        {
        // a faire
        //public DbSet<CartItem> CartItems { get; set; }

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }

        }
    }
