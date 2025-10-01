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
                var message = new MimeMessage();

                // ⬇️ يمكنك استخدام أي بريد كمرسل (ليس بالضرورة الذي سجلت به)
                message.From.Add(new MailboxAddress("Progres System", "alisce81@gmail.com"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    client.Timeout = 30000;

                    // ⬇️ إعدادات Brevo مع معلوماتك
                    await client.ConnectAsync("smtp-relay.brevo.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    // ⬇️ استخدم معلوماتك بالضبط كما حصلت عليها
                    await client.AuthenticateAsync("98477c001@smtp-brevo.com", "nH6Zq2IkGExXOL1D");

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation($"✅ تم إرسال البريد عبر Brevo إلى: {to}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ فشل إرسال البريد عبر Brevo إلى: {to}");
                throw new UserFriendlyException($"فشل إرسال البريد: {ex.Message}");
            }
        }

    
    }
}


