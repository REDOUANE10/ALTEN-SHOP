using Microsoft.EntityFrameworkCore;
using WebProduct.Data;
using WebProduct.Interfaces;
using WebProduct.Services;

namespace WebProduct.Extensions
    {
    public static class ApplicationServiceExtensions
        {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
            {

            services.AddDbContext<DataContext>(opt =>
            {

                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
                
            });
     
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;

            }


        }
    }
