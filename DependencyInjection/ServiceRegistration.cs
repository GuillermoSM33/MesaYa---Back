using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Services;

namespace MesaYa.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UserApiDb");

            // Registrar ApplicationDbContext con SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrar Servicios
            services.AddScoped<IUsuarioServices, UsuarioServices>();

            return services;
        }
    }
}
