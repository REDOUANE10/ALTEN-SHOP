using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api_Store.Entity;
using Api_Store.Interfaces;

namespace Api_Store.Services
    {
    public class TokenService : ITokenService
        {

        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
            {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            }
        public string CreateToken(User user)
            {
            var clmains = new List<Claim>
                    {
          //  new Claim(JwtRegisteredClaimNames.NameId, user.Username)
           new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username), // facultatif mais souvent utile
        new Claim(ClaimTypes.Email, user.Email)
                      };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
                {
                Subject = new ClaimsIdentity(clmains),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds

                };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


            }
        }
    }
