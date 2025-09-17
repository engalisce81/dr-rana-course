
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Lectures
{
    public class LectureAppService:ApplicationService
    {
        private readonly LectureManager _lectureManager;
        public LectureAppService(LectureManager lectureManager)
        {
            _lectureManager = lectureManager;
        }
        [Authorize(AcadmyPermissions.Lectures.View)]
        public async Task<ResponseApi<LectureDto>> GetAsync(Guid id) => await _lectureManager.GetAsync(id);
        [Authorize(AcadmyPermissions.Lectures.View)]
        public async Task<PagedResultDto<LectureDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _lectureManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Lectures.Create)]
        public async Task<ResponseApi<LectureDto>> CreateAsync(CreateUpdateLectureDto input) => await _lectureManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.Lectures.Edit)]
        public async Task<ResponseApi<LectureDto>> UpdateAsync(Guid id, CreateUpdateLectureDto input) => await _lectureManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Lectures.Delete)]
        public async Task DeleteAsync(Guid id) => await _lectureManager.DeleteAsync(id);
    }
}
