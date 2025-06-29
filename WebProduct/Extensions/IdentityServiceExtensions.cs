using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace Api_Store.Extensions
    {
    public static class IdentityServiceExtensions
        {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
            {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters
              {

              //**le serveur va verifier la clé de signature du jeton et s'assure
              // qu'elle est valide en fonction de la clé de signature
              //**//
              ValidateIssuerSigningKey = true,
              //spécidier ce qu'est notre clé de siganture de l'émetteur et nous allons dire que notre clé
              //de signature
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
              ValidateIssuer = false,
              ValidateAudience = false
              }
);
            return services;

            }
        }
    }
