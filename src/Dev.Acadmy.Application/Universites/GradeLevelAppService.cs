using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Universites
{
    public class GradeLevelAppService :ApplicationService
    {
        private readonly GradeLevelManager _gradelevelManager;
        public GradeLevelAppService(GradeLevelManager gradelevelManager)
        {
            _gradelevelManager = gradelevelManager;
        }
        [Authorize(AcadmyPermissions.GradeLevels.View)]
        public async Task<ResponseApi<GradeLevelDto>> GetAsync(Guid id) => await _gradelevelManager.GetAsync(id);
        [Authorize(AcadmyPermissions.GradeLevels.View)]
        public async Task<PagedResultDto<GradeLevelDto>> GetListAsync(int pageNumber, int pageSize, string? search, Guid collegeId) => await _gradelevelManager.GetListAsync(pageNumber, pageSize, search, collegeId);
        [Authorize(AcadmyPermissions.GradeLevels.Create)]
        public async Task<ResponseApi<GradeLevelDto>> CreateAsync(CreateUpdateGradeLevelDto input) => await _gradelevelManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.GradeLevels.Edit)]
        public async Task<ResponseApi<GradeLevelDto>> UpdateAsync(Guid id, CreateUpdateGradeLevelDto input) => await _gradelevelManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.GradeLevels.Delete)]
        public async Task DeleteAsync(Guid id) => await _gradelevelManager.DeleteAsync(id);
    }
}
