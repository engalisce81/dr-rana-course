using AutoMapper;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Data;
namespace Dev.Acadmy.Universites
{
    public class UniversityManager:DomainService
    {
        private readonly IRepository<University, Guid> _universityRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly CollegeManager _collegeManager;
        private readonly IIdentityUserRepository _userRepository;
        public UniversityManager(IIdentityUserRepository userRepository, CollegeManager collegeManager, ICurrentUser currentUser, IMapper mapper, IRepository<University, Guid> universityRepository)
        {
            _userRepository = userRepository;
            _collegeManager= collegeManager;
            _currentUser = currentUser;
            _universityRepository = universityRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<UniversityDto>> GetAsync(Guid id)
        {
            var university = await _universityRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (university == null) return new ResponseApi<UniversityDto> { Data = null, Success = false, Message = "Not found university" };
            var dto = _mapper.Map<UniversityDto>(university);
            return new ResponseApi<UniversityDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<UniversityDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _universityRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var universitys = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var universityDtos = _mapper.Map<List<UniversityDto>>(universitys);
            return new PagedResultDto<UniversityDto>(totalCount, universityDtos);
        }

        public async Task<ResponseApi<UniversityDto>> CreateAsync(CreateUpdateUniversityDto input)
        {
            var university = _mapper.Map<University>(input);
            var result = await _universityRepository.InsertAsync(university);
            var dto = _mapper.Map<UniversityDto>(result);
            return new ResponseApi<UniversityDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<UniversityDto>> UpdateAsync(Guid id, CreateUpdateUniversityDto input)
        {
            var universityDB = await _universityRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (universityDB == null) return new ResponseApi<UniversityDto> { Data = null, Success = false, Message = "Not found university" };
            var university = _mapper.Map(input, universityDB);
            var result = await _universityRepository.UpdateAsync(university);
            var dto = _mapper.Map<UniversityDto>(result);
            return new ResponseApi<UniversityDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var university = await _universityRepository.FirstOrDefaultAsync(x => x.Id == id);
            var users = await _userRepository.GetListAsync();
            foreach (var user in users) if (user.GetProperty<Guid>(SetPropConsts.UniversityId) == id) await _userRepository.DeleteAsync(id); 
            if (university == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found university" };
             var colleges = await _collegeManager.GetCollegesListAsync(id);
            foreach (var college in colleges.Items) { await _collegeManager.DeleteAsync(college.Id); }
            await _universityRepository.DeleteAsync(university);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetUniversitysListAsync()
        {
            var queryable = await _universityRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var universitys = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime));
            var universityDtos = _mapper.Map<List<LookupDto>>(universitys);
            return new PagedResultDto<LookupDto>(totalCount, universityDtos);
        }
    }
}
