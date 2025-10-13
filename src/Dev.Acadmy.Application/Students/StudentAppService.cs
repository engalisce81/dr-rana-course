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

namespace Dev.Acadmy.Students
{
    public class StudentAppService :ApplicationService
    {
        private readonly StudentManager _studentManager;
        public StudentAppService(StudentManager studentManager)
        {
            _studentManager = studentManager;
        }
        [Authorize(AcadmyPermissions.Students.Create)]
        public async Task CreateAsync(CreateUpdateStudentDto input) => await _studentManager.CreateStudentAsync(input);
        [Authorize(AcadmyPermissions.Students.Edit)]

        public async Task UpdateAsync(Guid id, CreateUpdateStudentDto input) => await _studentManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Students.Delete)]

        public async Task DeleteAsync(Guid id) => await _studentManager.DeleteAsync(id);
        [Authorize(AcadmyPermissions.Students.View)]
        public async Task<PagedResultDto<StudentDto>> GetStudentListAsync(int pageNumber, int pageSize, string? search = null) => await _studentManager.GetStudentListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Students.View)] 
        public async Task<ResponseApi<StudentDto>> GetAsync(Guid userId) => await _studentManager.GetAsync(userId);
    }
}
