using Microsoft.EntityFrameworkCore;
using Api_Store.Data;
using Api_Store.Interfaces;
using Api_Store.Services;

namespace Api_Store.Extensions
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
