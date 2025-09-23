using AutoMapper;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace Dev.Acadmy.Universites
{
    public  class GradeLevelManager : DomainService
    {
        private readonly IRepository<GradeLevel, Guid> _gradelevelRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public GradeLevelManager(ICurrentUser currentUser, IMapper mapper, IRepository<GradeLevel, Guid> gradelevelRepository)
        {
            _currentUser = currentUser;
            _gradelevelRepository = gradelevelRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<GradeLevelDto>> GetAsync(Guid id)
        {
            var gradelevel = await _gradelevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (gradelevel == null) return new ResponseApi<GradeLevelDto> { Data = null, Success = false, Message = "Not found gradelevel" };
            var dto = _mapper.Map<GradeLevelDto>(gradelevel);
            return new ResponseApi<GradeLevelDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<GradeLevelDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _gradelevelRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var gradelevels = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var gradelevelDtos = _mapper.Map<List<GradeLevelDto>>(gradelevels);
            return new PagedResultDto<GradeLevelDto>(totalCount, gradelevelDtos);
        }

        public async Task<ResponseApi<GradeLevelDto>> CreateAsync(CreateUpdateGradeLevelDto input)
        {
            var gradelevel = _mapper.Map<GradeLevel>(input);
            var result = await _gradelevelRepository.InsertAsync(gradelevel);
            var dto = _mapper.Map<GradeLevelDto>(result);
            return new ResponseApi<GradeLevelDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<GradeLevelDto>> UpdateAsync(Guid id, CreateUpdateGradeLevelDto input)
        {
            var gradelevelDB = await _gradelevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (gradelevelDB == null) return new ResponseApi<GradeLevelDto> { Data = null, Success = false, Message = "Not found gradelevel" };
            var gradelevel = _mapper.Map(input, gradelevelDB);
            var result = await _gradelevelRepository.UpdateAsync(gradelevel);
            var dto = _mapper.Map<GradeLevelDto>(result);
            return new ResponseApi<GradeLevelDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var gradelevel = await _gradelevelRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (gradelevel == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found gradelevel" };
            await _gradelevelRepository.DeleteAsync(gradelevel);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetGradeLevelsListAsync()
        {
            var queryable = await _gradelevelRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var gradelevels = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime));
            var gradelevelDtos = _mapper.Map<List<LookupDto>>(gradelevels);
            return new PagedResultDto<LookupDto>(totalCount, gradelevelDtos);
        }


    }
}

