using Dev.Acadmy.Exams;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.Courses
{
    public class ExamAppService:ApplicationService
    {
        private readonly ExamManager _examManager;
        public ExamAppService(ExamManager examManager) 
        {
            _examManager = examManager;
        }
        //[Authorize]
        //public async Task<ExamDto> GetAsync(Guid questionBankId, Guid examId) => await _examManager.GetAsync(questionBankId, examId);
        //[Authorize]
        //public async Task UpdateExam(Guid id, CreateUpdateExamDto input) => await _examManager.UpdateExam(id, input);

    }
}
