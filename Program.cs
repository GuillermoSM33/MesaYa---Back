using MesaYa.DependencyInjection;
using DotNetEnv;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Cargamos las variables de entorno desde .env
Env.Load();

// Recurrimos a los servicios registrados en ServiceRegistration
builder.Services.AddApplicationServices(builder.Configuration);

// Configuramos JSON para evitar ciclos de referencia
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

app.UseHttpsRedirection();
app.UseAuthentication(); // Autenticación con JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
