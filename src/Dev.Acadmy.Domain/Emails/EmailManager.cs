
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Email ,Guid> _emailRepository;

        public EmailManager(IRepository<Email, Guid> emailRepository, IHttpContextAccessor httpContextAccessor, IdentityUserManager userManager , IEmailSender emailSender)
        {
            _emailRepository = emailRepository;
            _httpContextAccessor = httpContextAccessor;
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
            var emailAdrress = input.EmailAdrees.Trim();
            var code = new Random().Next(1000, 9999).ToString();
            var email = await _emailRepository.FirstOrDefaultAsync(x => x.EmailAdrees == input.EmailAdrees);
            if (email != null) await _emailRepository.DeleteAsync(email);
            await _emailRepository.InsertAsync(new Email { EmailAdrees = emailAdrress, Code = code});
            try
            { 
                await _emailSender.SendAsync(emailAdrress, "Progres System Sent Code",code);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            return new ResponseApi<EmailDto> { Data = new EmailDto { Id = email.Id, Code = email.Code, EmailAdrees = email.EmailAdrees, IsAccept = true }, Success = true, Message = "code success" }; ;
        }
    }
}