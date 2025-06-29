using Api_Store.Enum;

namespace Api_Store.Entity
    {
    public class Product
        {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string InternalReference { get; set; }
        public int ShellId { get; set; }
        public InventoryStatus InventoryStatus { get; set; }
        public int Rating { get; set; }
        public long CreatedAt { get; set; }   
        public long UpdatedAt { get; set; }
        // Clé étrangère vers User
        public int UserId { get; set; }

        // Propriété de navigation : chaque produit appartient à un utilisateur
        public User User { get; set; }
        }
    }
