using Dev.Acadmy.Permissions;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Lectures
{
    public class LectureAppService:ApplicationService
    {
        private readonly LectureManager _lectureManager;
        private readonly QuizManager _quizManager;
        public LectureAppService(QuizManager quizManager, LectureManager lectureManager)
        {
            _quizManager = quizManager;
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
        [Authorize]
        public async Task<ResponseApi<QuizDetailsDto>> GetQuizDetailsAsync(Guid quizId) => await _lectureManager.GetQuizDetailsAsync(quizId);
        [Authorize]
        public async Task<ResponseApi<QuizResultDto>> CorrectQuizAsync(QuizAnswerDto input) => await _quizManager.CorrectQuizAsync(input);
        [Authorize]
        public async Task MarkQuizAsync(Guid quizId, int score) => await _quizManager.MarkQuizAsync(quizId, score);
    }
}
