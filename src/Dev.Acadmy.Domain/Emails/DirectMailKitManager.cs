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
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    _logger.LogInformation($"🔄 المحاولة {attempt}: إعداد رسالة البريد...");

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Progres System", "alisce81@gmail.com"));
                    message.To.Add(new MailboxAddress("", to));
                    message.Subject = subject;
                    message.Body = new TextPart("plain") { Text = body };

                    using (var client = new SmtpClient())
                    {
                        // ⬇️ إعدادات لتحسين الموثوقية
                        client.Timeout = 60000; // 60 ثانية
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        _logger.LogInformation("🔗 محاولة الاتصال بـ Brevo SMTP...");

                        // ⬇️ استخدام الإعدادات التي لديك مباشرة
                        await client.ConnectAsync("smtp-relay.brevo.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                        _logger.LogInformation("🔐 محاولة المصادقة...");

                        // ⬇️ استخدام بيانات الاعتماد الخاصة بك
                        await client.AuthenticateAsync("98477c001@smtp-brevo.com", "nH6Zq2IkGExXOL1D");

                        _logger.LogInformation("📤 إرسال البريد...");
                        await client.SendAsync(message);

                        await client.DisconnectAsync(true);
                    }

                    _logger.LogInformation($"✅ تم إرسال البريد بنجاح إلى: {to}");
                    return; // نجح - اخرج من الدالة
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ محاولة {attempt} فشلت");

                    if (attempt < 3)
                    {
                        var delaySeconds = 5 * attempt;
                        _logger.LogInformation($"⏳ انتظار {delaySeconds} ثواني قبل إعادة المحاولة...");
                        await Task.Delay(delaySeconds * 1000);
                    }
                    else
                    {
                        _logger.LogError($"💥 فشل جميع المحاولات الثلاث");
                        throw new UserFriendlyException($"فشل إرسال البريد بعد 3 محاولات. آخر خطأ: {ex.Message}");
                    }
                }
            }
        }
    }
}





