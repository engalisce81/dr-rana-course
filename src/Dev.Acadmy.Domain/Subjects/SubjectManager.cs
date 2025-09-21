using AutoMapper;
using Dev.Acadmy.Subjects;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
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
using Dev.Acadmy.Subjects;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.Colleges;
using Volo.Abp.Data;
using Microsoft.EntityFrameworkCore;

namespace Dev.Acadmy.Subjects
{
    public class SubjectManager : DomainService
    {
        private readonly IRepository<Subject ,Guid> _subjectRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<College, Guid> _collegeRepository;
        private readonly IIdentityUserRepository _userRepository;
        public SubjectManager(IIdentityUserRepository userRepository, IRepository<College, Guid> collegeRepository, ICurrentUser currentUser, IMapper mapper, IRepository<Subject , Guid> subjectRepository)
        {
            _userRepository = userRepository;
            _collegeRepository = collegeRepository;
            _currentUser = currentUser;
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<SubjectDto>> GetAsync(Guid id)
        {
            var subject = await _subjectRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (subject == null) return new ResponseApi<SubjectDto> { Data = null, Success = false, Message = "Not found subject" };
            var dto = _mapper.Map<SubjectDto>(subject);
            return new ResponseApi<SubjectDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<SubjectDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _subjectRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var subjects = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var subjectDtos = _mapper.Map<List<SubjectDto>>(subjects);
            return new PagedResultDto<SubjectDto>(totalCount, subjectDtos);
        }

        public async Task<ResponseApi<SubjectDto>> CreateAsync(CreateUpdateSubjectDto input)
        {
            var subject = _mapper.Map<Subject>(input);
            var result = await _subjectRepository.InsertAsync(subject);
            var dto = _mapper.Map<SubjectDto>(result);
            return new ResponseApi<SubjectDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<SubjectDto>> UpdateAsync(Guid id, CreateUpdateSubjectDto input)
        {
            var subjectDB = await _subjectRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (subjectDB == null) return new ResponseApi<SubjectDto> { Data = null, Success = false, Message = "Not found subject" };
            var subject = _mapper.Map(input, subjectDB);
            var result = await _subjectRepository.UpdateAsync(subject);
            var dto = _mapper.Map<SubjectDto>(result);
            return new ResponseApi<SubjectDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var subject = await _subjectRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (subject == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found subject" };
            await _subjectRepository.DeleteAsync(subject);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetSubjectsListAsync()
        {
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var collegeId = currentUser.GetProperty<Guid?>(SetPropConsts.CollegeId);
            if(collegeId == null) { }
            var subjects =await (await _collegeRepository.GetQueryableAsync()).Include(x=>x.Subjects).Select(x=>x.Subjects).ToListAsync();
            var subjectDtos = _mapper.Map<List<LookupDto>>(subjects);
            return new PagedResultDto<LookupDto>(subjectDtos.Count, subjectDtos);
        }
    }
}
