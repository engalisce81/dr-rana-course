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

namespace Dev.Acadmy.Exams
{
    public class ExamAppService:ApplicationService
    {
        private readonly ExamManager _examManager;
        public ExamAppService(ExamManager examManager)
        {
            _examManager = examManager;
        }
        [Authorize(AcadmyPermissions.Exams.View)]
        public async Task<ResponseApi<ExamDto>> GetAsync(Guid id) => await _examManager.GetAsync(id);
        [Authorize(AcadmyPermissions.Exams.View)]
        public async Task<PagedResultDto<ExamDto>> GetListAsync(int pageNumber, int pageSize, string? search) => await _examManager.GetListAsync(pageNumber, pageSize, search);
        [Authorize(AcadmyPermissions.Exams.Create)]
        public async Task<ResponseApi<ExamDto>> CreateAsync(CreateUpdateExamDto input) => await _examManager.CreateAsync(input);
        [Authorize(AcadmyPermissions.Exams.Edit)]
        public async Task<ResponseApi<ExamDto>> UpdateAsync(Guid id, CreateUpdateExamDto input) => await _examManager.UpdateAsync(id, input);
        [Authorize(AcadmyPermissions.Exams.Delete)]
        public async Task DeleteAsync(Guid id) => await _examManager.DeleteAsync(id);
        [Authorize]
        public async Task AddQuestionToExamAsync(CreateUpdateExamQuestionDto input) => await _examManager.AddQuestionToExam(input);
        [Authorize]
        public async Task<PagedResultDto<ExamQuestionsDto>> GetQuestionsFromBankAsync(List<Guid> bankIds, Guid examId)=> await _examManager.GetQuestionsFromBankAsync(bankIds, examId);
    }
}
