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
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Progres System", "alisce81@gmail.com"));
                    message.To.Add(new MailboxAddress("", to));
                    message.Subject = subject;
                    message.Body = new TextPart("plain") { Text = body };

                    using (var client = new SmtpClient())
                    {
                        client.Timeout = 60000;

                        // تجاهل التحقق من الشهادة SSL (لأغراض التصحيح فقط)
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        // حاول الاتصال بمنفذ 587 أولاً، ثم 465
                        try
                        {
                            _logger.LogInformation("محاولة الاتصال عبر المنفذ 587...");
                            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"فشل الاتصال عبر المنفذ 587: {ex.Message}");
                            _logger.LogInformation("محاولة الاتصال عبر المنفذ 465...");
                            await client.ConnectAsync("smtp.gmail.com", 465, true);
                        }

                        await client.AuthenticateAsync("alisce81@gmail.com", "كلمة_المرور_الجديدة"); // استخدم App Password هنا
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);

                        _logger.LogInformation($"✅ تم إرسال البريد بنجاح إلى: {to}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ فشل محاولة الإرسال {attempt} إلى: {to}");
                    if (attempt < 3)
                    {
                        await Task.Delay(2000 * attempt); // انتظر متزايد
                    }
                    else
                    {
                        throw new UserFriendlyException($"فشل إرسال البريد بعد 3 محاولات. الخطأ: {ex.Message}");
                    }
                }
            }

        }
    }
}


