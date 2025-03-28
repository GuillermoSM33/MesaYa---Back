using MesaYa.DependencyInjection;
using DotNetEnv;
using System.Text.Json.Serialization;
using MesaYa.Interfaces;
using MesaYa.Services;
using MesaYa.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env
Env.Load();

// Registrar servicios de la aplicación
builder.Services.AddApplicationServices(builder.Configuration);

// Agregar SignalR 
builder.Services.AddSignalR();

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

//Configuración del SendGrid
builder.Services.Configure<SendGridSettings>(options =>
{
    options.ApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
    options.FromEmail = Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL");
    options.FromName = Environment.GetEnvironmentVariable("SENDGRID_FROM_NAME");
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IRecaptchaValidator, RecaptchaService>();



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

//Mapa de SignalR
app.MapHub<MesaYa.Hubs.ReservaHub>("/reservaHub");

app.Run();
