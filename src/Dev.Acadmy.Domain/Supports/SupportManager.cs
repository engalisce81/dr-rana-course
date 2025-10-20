using AutoMapper;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using Dev.Acadmy.Universites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.Supports
{
    public class SupportManager :DomainService
    {
        private readonly IRepository<Support, Guid> _supportRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;
        public SupportManager(IIdentityUserRepository userRepository, ICurrentUser currentUser, IMapper mapper, IRepository<Support, Guid> supportRepository)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _supportRepository = supportRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<SupportDto>> GetAsync(Guid id)
        {
            var support = await _supportRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (support == null) return new ResponseApi<SupportDto> { Data = null, Success = false, Message = "Not found support" };
            var dto = _mapper.Map<SupportDto>(support);
            return new ResponseApi<SupportDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<ResponseApi<SupportDto>> GetSupportAsync()
        {
            var support = await _supportRepository.FirstOrDefaultAsync();
            if (support == null) return new ResponseApi<SupportDto> { Data = null, Success = false, Message = "Not found support" };
            var dto = _mapper.Map<SupportDto>(support);
            return new ResponseApi<SupportDto> { Data = dto, Success = true, Message = "find succeess" };
        }
        public async Task<PagedResultDto<SupportDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _supportRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.FaceBook.Contains(search)|| c.Email.Contains(search) || c.PhoneNumber.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var supports = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var supportDtos = _mapper.Map<List<SupportDto>>(supports);
            return new PagedResultDto<SupportDto>(totalCount, supportDtos);
        }

        public async Task<ResponseApi<SupportDto>> CreateAsync(CreateUpdateSupportDto input)
        {
            var support = _mapper.Map<Support>(input);
            var result = await _supportRepository.InsertAsync(support);
            var dto = _mapper.Map<SupportDto>(result);
            return new ResponseApi<SupportDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<SupportDto>> UpdateAsync(Guid id, CreateUpdateSupportDto input)
        {
            var supportDB = await _supportRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (supportDB == null) return new ResponseApi<SupportDto> { Data = null, Success = false, Message = "Not found support" };
            var support = _mapper.Map(input, supportDB);
            var result = await _supportRepository.UpdateAsync(support);
            var dto = _mapper.Map<SupportDto>(result);
            return new ResponseApi<SupportDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var support = await _supportRepository.FirstOrDefaultAsync(x => x.Id == id);
            await _supportRepository.DeleteAsync(support);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

      
    }
}
