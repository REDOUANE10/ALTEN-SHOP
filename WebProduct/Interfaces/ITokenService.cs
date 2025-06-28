using WebProduct.Entity;

namespace WebProduct.Interfaces
    {
    public interface ITokenService
        {
        string CreateToken(User user );
        }
    }
