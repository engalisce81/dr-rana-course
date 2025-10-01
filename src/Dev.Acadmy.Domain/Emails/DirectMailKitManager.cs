using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace Dev.Acadmy.Emails
{
    public class DirectMailKitManager : DomainService
    {
        private readonly ILogger<DirectMailKitManager> _logger;

        public DirectMailKitManager(ILogger<DirectMailKitManager> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var apiKey = "nH6Zq2IkGExXOL1D";
                var apiUrl = "https://api.brevo.com/v3/smtp/email";

                var emailData = new
                {
                    sender = new { email = "alisce81@gmail.com", name = "Progres System" },
                    to = new[] { new { email = to } },
                    subject = subject,
                    htmlContent = $"<p>{body}</p>",
                    textContent = body
                };

                var json = JsonSerializer.Serialize(emailData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // ⬇️ استخدام HttpClient مباشرة بدون Factory
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("api-key", apiKey);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    var response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"✅ تم إرسال البريد عبر Brevo API إلى: {to}");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Brevo API Error: {response.StatusCode} - {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ فشل إرسال البريد عبر Brevo API");
                throw new UserFriendlyException($"فشل إرسال البريد: {ex.Message}");
            }
        }
    }

}



