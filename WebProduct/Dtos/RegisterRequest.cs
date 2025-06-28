using System.ComponentModel.DataAnnotations;

namespace WebProduct.Dtos
    {
    public class RegisterDto
        {
        [Required]
        public string  Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        public string Password { get; set; }


        [Required]
        public string Username { get; set; }
        [Required]
        public string Firstname { get; set; }
        }
    }
