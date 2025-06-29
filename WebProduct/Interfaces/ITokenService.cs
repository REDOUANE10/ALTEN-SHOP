using Api_Store.Entity;

namespace Api_Store.Interfaces
    {
    public interface ITokenService
        {
        string CreateToken(User user );
        }
    }
