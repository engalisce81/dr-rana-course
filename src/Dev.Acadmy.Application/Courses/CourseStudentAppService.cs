
using Dev.Acadmy.Permissions;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Courses
{
    public class CourseStudentAppService:ApplicationService
    {
        private readonly CourseStudentManager _coursestudentManager;
        public CourseStudentAppService(CourseStudentManager coursestudentManager)
        {
            _coursestudentManager = coursestudentManager;
        }
        [Authorize(AcadmyPermissions.CourseStudents.View)]
        public async Task<ResponseApi<CourseStudentDto>> GetAsync(Guid id) => await _coursestudentManager.GetAsync(id);
        [Authorize(AcadmyPermissions.CourseStudents.View)]
        public async Task<PagedResultDto<CourseStudentDto>> GetListAsync(int pageNumber, int pageSize, bool isSubscribe, Guid courseId, string? search) => await _coursestudentManager.GetListAsync(pageNumber, pageSize, isSubscribe,courseId, search);
        [Authorize(AcadmyPermissions.CourseStudents.Create)]
        public async Task<ResponseApi<CourseStudentDto>> CreateAsync(CreateUpdateCourseStudentDto input) => await _coursestudentManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.CourseStudents.Edit)]
        public async Task<ResponseApi<CourseStudentDto>> UpdateAsync(Guid id, CreateUpdateCourseStudentDto input) => await _coursestudentManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.CourseStudents.Delete)]
        public async Task DeleteAsync(Guid id) => await _coursestudentManager.DeleteAsync(id);
        [Authorize(AcadmyPermissions.CourseStudents.Delete)]
        public async Task DeleteAllStudentInCourse(Guid courseId) => await _coursestudentManager.DeleteAllStudentInCourse(courseId);
        [Authorize]
        public async Task<PagedResultDto<StudentDegreeByCourseDto>> GetStudentDegreByCourseAsync(int pageNumber, int pageSize, Guid courseId, Guid userId) => await _coursestudentManager.GetStudentDegreByCourseAsync(pageNumber, pageSize, courseId, userId);

    }
}
