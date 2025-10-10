using Dev.Acadmy.Questions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.Exams
{
    public class ExamManager:DomainService
    {
        private readonly IRepository<Exam ,Guid> _examRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<QuestionBank, Guid> _questionBankRepository;
        public ExamManager(IRepository<QuestionBank, Guid> questionBankRepository, IRepository<Exam,Guid> examRepository , IRepository<Question, Guid> questionRepository)
        {
            _questionBankRepository = questionBankRepository;
            _questionRepository = questionRepository;
            _examRepository = examRepository;
        }

        public async Task<ExamDto> GetAsync(Guid questionBankId ,Guid examId)
        {
            var questionBank =await (await _questionBankRepository.GetQueryableAsync()).Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == questionBankId);
            if (questionBank == null) throw new Exception("Question Bank not found");
            var exam = await _examRepository.GetAsync(examId);
            if (exam == null) throw new Exception("Exam not found");
            var examDto =new ExamDto
            {
                Id = examId,
                Name = exam.Name,
                TimeExam = exam.TimeExam,
                IsActive = exam.IsActive,
            };
            foreach (var question in questionBank.Questions)
            {
                if (question.ExamId == examId) examDto.ExamQuestions.Add(new ExamQuestions { Id = question.Id, IsSelected = true });
                else examDto.ExamQuestions.Add(new ExamQuestions { Id = question.Id, IsSelected = false });
                return examDto;
            }
            return examDto;
        }

        public  async Task UpdateExam(Guid id, CreateUpdateExamDto input)
        {
            var exam = await _examRepository.GetAsync(id);
            exam.Name = input.Name;
            exam.TimeExam = input.TimeExam;
            exam.IsActive = input.IsActive;
            foreach (var questionId in input.QuestionIds)
            {
                var question = await _questionRepository.GetAsync(questionId);
                question.ExamId = id;
                await _questionRepository.UpdateAsync(question,autoSave:true);
            }
            await _examRepository.UpdateAsync(exam);
        }
    }
}
