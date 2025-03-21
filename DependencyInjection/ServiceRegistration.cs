using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using MesaYa.Models;
using MesaYa.Hubs;
using System.IdentityModel.Tokens.Jwt;

namespace MesaYa.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Cargamos variables de entorno desde .env
            Env.Load();

            // Construimos una nueva configuración que combine .env y appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var config = configurationBuilder.Build();

            //Asignamos manualmente las claves JWT desde .env para asegurarnos de que no sean null
            config["Jwt:Key"] = Env.GetString("JWT_SECRET");
            config["Jwt:Issuer"] = Env.GetString("JWT_ISSUER");
            config["Jwt:Audience"] = Env.GetString("JWT_AUDIENCE");
            config["Jwt:ExpiryMinutes"] = Env.GetString("JWT_EXPIRY_MINUTES", "60"); // 60 min por defecto

            //Registramos IConfiguration corregida para que esté disponible en todos los servicios
            services.AddSingleton<IConfiguration>(config);

            var connectionString = config.GetConnectionString("UserApiDb");

            //Configuración de Entity Framework con SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrar servicios
            services.AddScoped<IUsuarioServices, UsuarioServices>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IRestauranteService, RestauranteService>();
            services.AddScoped<IMesaService, MesaService>();
            services.AddScoped<IMenuCategoriaService, MenuCategoriaService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            // Agregar el Hub de SignalR
            services.AddTransient<ReservaHub>();

            // Configuración de autenticación JWT
            var jwtKey = config["Jwt:Key"];
            var jwtIssuer = config["Jwt:Issuer"];
            var jwtAudience = config["Jwt:Audience"];
            var jwtExpiryMinutes = int.Parse(config["Jwt:ExpiryMinutes"]);

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new Exception("JWT_SECRET no está definido en .env o es null.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var jwtToken = context.SecurityToken as JwtSecurityToken;
                            var jti = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                            if (string.IsNullOrEmpty(jti))
                            {
                                context.Fail("El token no tiene 'jti' y no puede validarse.");
                                return;
                            }

                            var dbContext = context.HttpContext.RequestServices
                                .GetRequiredService<ApplicationDbContext>();

                            bool isRevoked = await dbContext.RevokedTokens
                                .AnyAsync(rt => rt.Jti == jti);

                            if (isRevoked)
                            {
                                context.Fail("Token revocado. No se permite el acceso.");
                                return;
                            }
                        }
                    };
                });

            // Configuración de Swagger con autenticación JWT
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MesaYa API", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Ingrese el token JWT en el formato: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                options.AddSecurityDefinition("Bearer", securitySchema);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new string[] { } }
                });
            });

            return services;
        }
    }
}
