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
                        // ⬇️ إعدادات متقدمة للشبكة
                        client.Timeout = 45000; // 45 ثانية
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        _logger.LogInformation($"🔄 المحاولة {attempt}: الاتصال بـ Brevo...");

                        // حاول منافذ مختلفة
                        try
                        {
                            await client.ConnectAsync("smtp-relay.brevo.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                            _logger.LogInformation("✅ متصل عبر المنفذ 587");
                        }
                        catch (Exception ex1)
                        {
                            _logger.LogWarning($"❌ المنفذ 587 فشل: {ex1.Message}");

                            // جرب المنفذ 465
                            try
                            {
                                await client.ConnectAsync("smtp-relay.brevo.com", 465, true);
                                _logger.LogInformation("✅ متصل عبر المنفذ 465");
                            }
                            catch (Exception ex2)
                            {
                                _logger.LogWarning($"❌ المنفذ 465 فشل: {ex2.Message}");

                                // جرب المنفذ 25
                                try
                                {
                                    await client.ConnectAsync("smtp-relay.brevo.com", 25, MailKit.Security.SecureSocketOptions.StartTls);
                                    _logger.LogInformation("✅ متصل عبر المنفذ 25");
                                }
                                catch (Exception ex3)
                                {
                                    _logger.LogWarning($"❌ المنفذ 25 فشل: {ex3.Message}");
                                    throw new Exception($"كل المنافذ فشلت: 587->{ex1.Message}, 465->{ex2.Message}, 25->{ex3.Message}");
                                }
                            }
                        }

                        await client.AuthenticateAsync("98477c001@smtp-brevo.com", "nH6Zq2IkGExXOL1D");
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);

                        _logger.LogInformation($"✅ تم إرسال البريد بنجاح إلى: {to}");
                        return; // نجح - اخرج
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"❌ محاولة {attempt} فشلت");

                    if (attempt < 3)
                    {
                        await Task.Delay(3000 * attempt); // انتظر 3, 6, 9 ثواني
                        _logger.LogInformation($"⏳ إعادة المحاولة بعد {3000 * attempt} مللي ثانية...");
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


