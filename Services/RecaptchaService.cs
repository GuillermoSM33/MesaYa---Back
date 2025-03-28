using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MesaYa.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Azure.Core;

namespace MesaYa.Services
{
    public class RecaptchaService : IRecaptchaValidator
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;

        public RecaptchaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _secretKey = configuration["RECAPTCHA_SECRET_KEY"]
                         ?? throw new Exception("RECAPTCHA_SECRET_KEY no está configurado.");
        }

        public async Task<bool> ValidateAsync(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("El token de reCAPTCHA está vacío.");
                    return false;
                }
                string trimmedToken = token.Trim();

                var parameters = new Dictionary<string, string>
                {
                    { "secret", _secretKey },
                    { "response", trimmedToken }
                };

                var content = new FormUrlEncodedContent(parameters);

                var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Respuesta de Google: {jsonResponse}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validando reCAPTCHA: {ex.Message}");
                return false;
            }
        }

        private class RecaptchaResponse
        {
            public bool Success { get; set; }
            public double Score { get; set; }
            public string Action { get; set; }
            public string ChallengeTs { get; set; }
            public string Hostname { get; set; }
        }
    }
}
