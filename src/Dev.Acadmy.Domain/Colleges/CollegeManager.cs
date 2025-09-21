using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Dev.Acadmy.Response;
using Dev.Acadmy.LookUp;
namespace Dev.Acadmy.Colleges
{
    public class CollegeManager :DomainService
    {
        private readonly IRepository<College ,Guid> _collegeRepository;
        private readonly IMapper _mapper;
        public CollegeManager(IMapper mapper, IRepository<College,Guid> collegeRepository) 
        {
            _collegeRepository = collegeRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApi<CollegeDto>> GetAsync(Guid id)
        {
            var college = await _collegeRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (college == null)  return  new ResponseApi<CollegeDto> { Data = null, Success = false, Message = "Not found college" };
            var dto = _mapper.Map<CollegeDto>(college);
            return new ResponseApi<CollegeDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<CollegeDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _collegeRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Name.Contains(search));  
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var colleges = await AsyncExecuter.ToListAsync( queryable.OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize) .Take(pageSize) );
            var collegeDtos = _mapper.Map<List<CollegeDto>>(colleges);
            return new PagedResultDto<CollegeDto>(totalCount, collegeDtos);
        }

        public async Task<ResponseApi<CollegeDto>> CreateAsync(CreateUpdateCollegeDto input)
        {
            var college= _mapper.Map<College>(input);
            var result = await _collegeRepository.InsertAsync(college);
            var dto = _mapper.Map<CollegeDto>(result);
            return new ResponseApi<CollegeDto> { Data=dto,Success=true ,Message= "save succeess"};
        }

        public async Task<ResponseApi<CollegeDto>> UpdateAsync(Guid id, CreateUpdateCollegeDto input)
        {
            var collegeDB = await _collegeRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (collegeDB == null) return new ResponseApi<CollegeDto> { Data = null, Success = false, Message = "Not found college" };
            var college = _mapper.Map(input, collegeDB);
            var result = await _collegeRepository.UpdateAsync(college);
            var dto = _mapper.Map<CollegeDto>(result);
            return new ResponseApi<CollegeDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var college = await _collegeRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (college == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found college" };
            await _collegeRepository.DeleteAsync(college);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetCollegesListAsync()
        {
            var queryable = await _collegeRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var colleges = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime));
            var collegeDtos = _mapper.Map<List<LookupDto>>(colleges);
            return new PagedResultDto<LookupDto>(totalCount, collegeDtos);
        }


    }
}
