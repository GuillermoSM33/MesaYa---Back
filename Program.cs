using MesaYa.DependencyInjection;
using DotNetEnv;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();
Console.WriteLine($"JWT_SECRET desde .env: {Environment.GetEnvironmentVariable("JWT_SECRET")}");

// Registrar servicios de la aplicación
builder.Services.AddApplicationServices(builder.Configuration);

// Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173") //Se especifica la URL que usamos en el front
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

// Configurar JSON para evitar ciclos de referencia
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplicar middleware
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");  // Nos aseguramos que se use CORS antes de Authentication y Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
