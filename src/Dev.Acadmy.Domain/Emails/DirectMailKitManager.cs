using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.Emails
{
    public class DirectMailKitManager: DomainService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Progres System", "alisce81@gmail.com"));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    // إعدادات متقدمة للاتصال
                    client.Timeout = 60000; // 60 ثانية

                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    await client.AuthenticateAsync("alisce81@gmail.com", "rfyuvybdbrziowgs");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"فشل إرسال البريد: {ex.Message}");
            }
        }
    }
}
