namespace Api_Store.Entity
    {
    public class User
        {
       public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        public ICollection<Product> Products { get; set; }
        }
    }
