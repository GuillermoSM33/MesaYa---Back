using MesaYa.Configurations;
using MesaYa.Data;
using MesaYa.DTOs;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MesaYa.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly SendGridSettings _sendGridSettings;

        public NotificacionService(ApplicationDbContext context, IConfiguration configuration, IOptions<SendGridSettings> sendGridSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration;
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task<Notificacion> CrearNotificacionAsync(int usuarioId, string mensaje, string tipo)
        {
            var notificacion = new Notificacion
            {
                UsuarioId = usuarioId,
                Mensaje = mensaje,
                Tipo = tipo,
                Enviado = false,
                FechaEnvio = null
            };


            await _context.Notificaciones.AddAsync(notificacion);
            await _context.SaveChangesAsync();

            return notificacion;
        }

        /*public async Task EnviarNotificacionAsync(Notificacion notificacion)
        {
            var usuario = await GetUserByIdAsync(notificacion.UsuarioId);

            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
            var subject = "Notificación de Reserva";
            var to = new EmailAddress(usuario.Email);
            var plainTextContent = notificacion.Mensaje;
            var htmlContent = $"<strong>{notificacion.Mensaje}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                notificacion.Enviado = true;
                notificacion.FechaEnvio = DateTime.Now;
                _context.Notificaciones.Update(notificacion);
                await _context.SaveChangesAsync();
            }
        }

        */
        public async Task EnviarNotificacionAsync(Notificacion notificacion, byte[] pdfAdjunto, string nombreArchivo)
        {
            var usuario = await GetUserByIdAsync(notificacion.UsuarioId);

            var client = new SendGridClient(_sendGridSettings.ApiKey);
            var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
            var subject = "🎉 Confirmación de Reserva";
            var to = new EmailAddress(usuario.Email);

            var plainTextContent = notificacion.Mensaje;
            var htmlContent = $"<strong>{notificacion.Mensaje}</strong>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            // 👇 Adjuntar el PDF
            var base64Pdf = Convert.ToBase64String(pdfAdjunto);
            msg.AddAttachment(nombreArchivo, base64Pdf, "application/pdf");

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                notificacion.Enviado = true;
                notificacion.FechaEnvio = DateTime.Now;
                _context.Notificaciones.Update(notificacion);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task EnviarNotificacionAsync(NotificacionDto notificacion)
        //{
        //    var usuario = await GetUserByIdAsync(notificacion.NombreUsuario.leng);

        //    var client = new SendGridClient(_sendGridSettings.ApiKey);
        //    var from = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
        //    var subject = "🎉 Tu reserva ha sido confirmada en MesaYa!";
        //    var to = new EmailAddress(usuario.Email);

        //    // 📨 Mensaje en texto plano
        //    var plainTextContent = $"Hola {notificacion.NombreUsuario},\n\n" +
        //        $"Tu reserva ha sido exitosa en {notificacion.RestauranteNombre}.\n" +
        //        $"Detalles de la reserva:\n" +
        //        $"- 📅 Fecha: {notificacion.FechaReserva:dd/MM/yyyy HH:mm}\n" +
        //        $"- 🍽️ Número de mesas: {notificacion.NumeroMesas}\n" +
        //        $"- 👥 Número de personas: {notificacion.NumeroPersonas}\n\n" +
        //        $"Gracias por reservar con nosotros.\n" +
        //        $"Equipo MesaYa";

        //    // 📨 Mensaje en HTML
        //    var htmlContent = $@"
        //<html>
        //<body style='font-family: Arial, sans-serif;'>
        //    <h2 style='color: #007bff;'>🎉 ¡Tu reserva ha sido confirmada!</h2>
        //    <p>Hola <strong>{notificacion.NombreUsuario}</strong>,</p>
        //    <p>Tu reserva en <strong>{notificacion.RestauranteNombre}</strong> ha sido exitosa.</p>
        //    <p><strong>📅 Fecha:</strong> {notificacion.FechaReserva:dd/MM/yyyy HH:mm}</p>
        //    <p><strong>🍽️ Número de mesas:</strong> {notificacion.NumeroMesas}</p>
        //    <p><strong>👥 Número de personas:</strong> {notificacion.NumeroPersonas}</p>
        //    <hr>
        //    <p style='color: gray;'>Gracias por reservar con nosotros.</p>
        //    <p><strong>Equipo MesaYa</strong></p>
        //</body>
        //</html>";

        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);

        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        notificacion.Enviado = true;
        //        notificacion.FechaEnvio = DateTime.Now;
        //    }
        //}
        private async Task<Usuario> GetUserByIdAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return usuario;
        }



    }
}