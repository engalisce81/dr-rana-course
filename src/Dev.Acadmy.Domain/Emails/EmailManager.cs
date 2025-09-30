
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;



namespace Dev.Acadmy.Emails
{
  
    public class EmailManager: DomainService
    {
        private readonly IEmailSender _emailSender;
        private readonly IdentityUserManager _userManager;
        private readonly IRepository<Email ,Guid> _emailRepository;

        public EmailManager(IRepository<Email, Guid> emailRepository, IdentityUserManager userManager , IEmailSender emailSender)
        {
            _emailRepository = emailRepository;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<ResponseApi<EmailDto>> CheckCodeAsync(UpdateEmailDto input)
        {
            var email = await _emailRepository.FirstOrDefaultAsync(x => x.EmailAdrees == input.EmailAdrees);
            if(email == null) throw new  UserFriendlyException("the email not found");
            if (email.Code == input.Code) { return new ResponseApi<EmailDto> { Data = new EmailDto { Id = email.Id, Code = email.Code, EmailAdrees = email.EmailAdrees, IsAccept = true }, Success = true, Message = "code success" }; }
            else return new ResponseApi<EmailDto> { Data = null, Success = true, Message = "the code no accept check email" };
            
        }



        public async Task<ResponseApi<EmailDto>> SendNotificationToEmailAsync(CreateEmailDto input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.EmailAdrees))
                throw new UserFriendlyException("Email address is required");

            var emailAdrress = input.EmailAdrees.Trim();

            // Check if email is already used by a user
            var user = await _userManager.FindByEmailAsync(emailAdrress);
            if (user != null)
                throw new UserFriendlyException("The email is used by another account");

            // Generate verification code
            var code = new Random().Next(1000, 9999).ToString();

            // Remove old record if exists
            var oldEmail = await _emailRepository.FirstOrDefaultAsync(x => x.EmailAdrees == emailAdrress);
            if (oldEmail != null)
                await _emailRepository.DeleteAsync(oldEmail);

            // Insert new email record and capture it
            var newEmail = await _emailRepository.InsertAsync(new Email
            {
                EmailAdrees = emailAdrress,
                Code = code
            }, autoSave: true);

            // Send email
            try
            {
                await _emailSender.SendAsync(emailAdrress, "Progres System Sent Code", code);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Failed to send email: {ex.Message}");
            }

            // Return response using the new email entity
            return new ResponseApi<EmailDto>
            {
                Data = new EmailDto
                {
                    Id = newEmail.Id,
                    Code = newEmail.Code,
                    EmailAdrees = newEmail.EmailAdrees,
                    IsAccept = false
                },
                Success = true,
                Message = "Code sent successfully"
            };
        }

    }
}