using Dev.Acadmy.LookUp;
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Universites
{
    public class UniversityAppService :ApplicationService
    {
        private readonly UniversityManager _universityManager;
        public UniversityAppService(UniversityManager universityManager)
        {
            _universityManager = universityManager;
        }
        [Authorize(AcadmyPermissions.Universites.View)]
        public async Task<ResponseApi<UniversityDto>> GetAsync(Guid id) => await _universityManager.GetAsync(id);
        [Authorize(AcadmyPermissions.Universites.View)]
        public async Task<PagedResultDto<UniversityDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _universityManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Universites.Create)]
        public async Task<ResponseApi<UniversityDto>> CreateAsync(CreateUpdateUniversityDto input) => await _universityManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.Universites.Edit)]
        public async Task<ResponseApi<UniversityDto>> UpdateAsync(Guid id, CreateUpdateUniversityDto input) => await _universityManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Universites.Delete)]
        public async Task DeleteAsync(Guid id) => await _universityManager.DeleteAsync(id);
        [AllowAnonymous]
        public async Task<PagedResultDto<LookupDto>> GetUniversitysListAsync() => await _universityManager.GetUniversitysListAsync();

    }
}
