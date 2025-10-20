using Dev.Acadmy.LookUp;
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Dev.Acadmy.Universites;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Supports
{
    public class SupportAppService :ApplicationService
    {
        private readonly SupportManager _supportManager;
        public SupportAppService(SupportManager supportManager)
        {
            _supportManager = supportManager;
        }
        [Authorize(AcadmyPermissions.Supports.View)]
        public async Task<ResponseApi<SupportDto>> GetAsync(Guid id) => await _supportManager.GetAsync(id);
        [Authorize(AcadmyPermissions.Supports.View)]
        public async Task<PagedResultDto<SupportDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _supportManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Supports.Create)]
        public async Task<ResponseApi<SupportDto>> CreateAsync(CreateUpdateSupportDto input) => await _supportManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.Supports.Edit)]
        public async Task<ResponseApi<SupportDto>> UpdateAsync(Guid id, CreateUpdateSupportDto input) => await _supportManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Supports.Delete)]
        public async Task DeleteAsync(Guid id) => await _supportManager.DeleteAsync(id);
        [AllowAnonymous]
        public async Task<ResponseApi<SupportDto>> GetSupportAsync() => await _supportManager.GetSupportAsync();
    }
}
