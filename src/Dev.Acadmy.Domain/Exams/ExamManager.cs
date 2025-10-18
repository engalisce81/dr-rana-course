using AutoMapper;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.Exams
{
    public class ExamManager:DomainService
    {
        private readonly IRepository<Exam ,Guid> _examRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<QuestionBank, Guid> _questionBankRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<ExamQuestionBank, Guid> _examQuestionBankRepository;
        public ExamManager(IRepository<ExamQuestionBank, Guid> examQuestionBankRepository, ICurrentUser currentUser , IIdentityUserRepository userRepository, IRepository<QuestionBank, Guid> questionBankRepository, IMapper mapper, IRepository<Exam,Guid> examRepository , IRepository<Question, Guid> questionRepository)
        {
            _examQuestionBankRepository = examQuestionBankRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _questionBankRepository = questionBankRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _examRepository = examRepository;
        }

        public async Task<ResponseApi<ExamDto>> GetAsync(Guid id)
        {
            // نجيب examQuestionBanks المرتبطة بالامتحان
            var examQuestionBanks = await (await _examQuestionBankRepository.GetQueryableAsync())
                .Where(x => x.ExamId == id)
                .Include(x => x.Exam)
                .Include(x => x.QuestionBank)
                    .ThenInclude(qb => qb.Questions)
                        .ThenInclude(q => q.QuestionAnswers)
                .ToListAsync();

            // لو مفيش حاجة
            if (!examQuestionBanks.Any())
                return new ResponseApi<ExamDto>
                {
                    Success = false,
                    Message = "Exam not found",
                    Data = null
                };

            // ناخد أول exam كمرجع (كلهم لنفس الامتحان)
            var examEntity = examQuestionBanks.First().Exam;

            // نبني ExamDto واحد فقط
            var dto = new ExamDto
            {
                Id = examEntity.Id,
                Name = examEntity.Name,
                TimeExam = examEntity.TimeExam,
                IsActive = examEntity.IsActive,
                ExamQuestions = examQuestionBanks
                    .SelectMany(eq => eq.QuestionBank.Questions)
                    .Select(q => new ExamQuestions
                    {
                        Id = q.Id,
                        Tittle = q.Title,
                        QuestionType = q.QuestionType.Name,
                        QuestionAnswers = q.QuestionAnswers.Select(qa => new ExamQuestionAnswer
                        {
                            AnswerId = qa.Id,
                            Answer = qa.Answer,
                            IsSelected = qa.IsCorrect
                        }).ToList()
                    })
                    .ToList()
            };

            return new ResponseApi<ExamDto>
            {
                Data = dto,
                Success = true,
                Message = "Exam loaded successfully"
            };
        }

        
        public async Task<PagedResultDto<ExamDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _examRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x => x.Course).Where(c => c.Name.Contains(search) || c.Course.Name.Contains(search));
            var exams = new List<Exam>();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            if (roles.Any(x => x.Name.ToUpper() == RoleConsts.Admin.ToUpper())) exams = await AsyncExecuter.ToListAsync(queryable.Include(x => x.Course).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else exams = await AsyncExecuter.ToListAsync(queryable.Where(c => c.CreatorId == _currentUser.GetId()).Include(x => x.Course).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var examDtos = _mapper.Map<List<ExamDto>>(exams);
            return new PagedResultDto<ExamDto>(totalCount, examDtos);
        }

        public async Task<ResponseApi<ExamDto>> CreateAsync(CreateUpdateExamDto input)
        {
            var exam = _mapper.Map<Exam>(input);
            var result = await _examRepository.InsertAsync(exam, autoSave: true);
            await CreateRelation(result, input);
            var dto = _mapper.Map<ExamDto>(result);
            return new ResponseApi<ExamDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<ExamDto>> UpdateAsync(Guid id, CreateUpdateExamDto input)
        {
            var examDB = await _examRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (examDB == null) return new ResponseApi<ExamDto> { Data = null, Success = false, Message = "Not found exam" };
            var exam = _mapper.Map(input, examDB);
            var result = await _examRepository.UpdateAsync(exam);
            await DeleteRelation(id);
            await CreateRelation(result, input);
            var dto = _mapper.Map<ExamDto>(result);
            return new ResponseApi<ExamDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (exam == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found exam" };
            await DeleteRelation(id);
            await _examRepository.DeleteAsync(exam);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task DeleteRelation(Guid examId)
        {
            var examQuestionBanks = await _examQuestionBankRepository.GetListAsync(x => x.ExamId == examId);
            var questions = await _questionRepository.GetListAsync(x => x.ExamId == examId);
            foreach (var question in questions) question.ExamId = null;
            await _examQuestionBankRepository.DeleteManyAsync(examQuestionBanks, autoSave: true);
            await _questionRepository.UpdateManyAsync(questions, autoSave: true);
        }

        public async Task CreateRelation(Exam result , CreateUpdateExamDto input)
        {
            foreach (var questionBankId in input.QuestionBankIds)
            {
                var examQuestionBank = new ExamQuestionBank
                {
                    ExamId = result.Id,
                    QuestionBankId = questionBankId
                };
                await _examQuestionBankRepository.InsertAsync(examQuestionBank, autoSave: true);
            }

            foreach (var questionId in input.QuestionIds)
            {
                var question = await _questionRepository.FirstOrDefaultAsync(x => x.Id == questionId);
                if (question != null)
                {
                    question.ExamId = result.Id;
                    await _questionRepository.UpdateAsync(question, autoSave: true);
                }
            }
        }

        //public async Task<ExamDto> GetAsync(Guid examId ,Guid examId)
        //{
        //    var exam =await (await _examRepository.GetQueryableAsync()).Include(x => x.Questions).FirstOrDefaultAsync(x => x.Id == examId);
        //    if (exam == null) throw new Exception("Question Bank not found");
        //    var exam = await _examRepository.GetAsync(examId);
        //    if (exam == null) throw new Exception("Exam not found");
        //    var examDto =new ExamDto
        //    {
        //        Id = examId,
        //        Name = exam.Name,
        //        TimeExam = exam.TimeExam,
        //        IsActive = exam.IsActive,
        //    };
        //    foreach (var question in exam.Questions)
        //    {
        //        if (question.ExamId == examId) examDto.ExamQuestions.Add(new ExamQuestions { Id = question.Id,Tittle = question.Title ,IsSelected = true });
        //        else examDto.ExamQuestions.Add(new ExamQuestions { Id = question.Id, Tittle = question.Title, IsSelected = false });
        //    }
        //    return examDto;
        //}

        //public async Task UpdateExam(Guid id, CreateUpdateExamDto input)
        //{
        //    var exam = await _examRepository.GetAsync(id);

        //    // تحديث بيانات الامتحان
        //    exam.Name = input.Name;
        //    exam.TimeExam = input.TimeExam;
        //    exam.IsActive = input.IsActive;

        //    // 🟢 جلب جميع الأسئلة التابعة للامتحان الحالي
        //    var allQuestions = await _questionRepository.GetListAsync();

        //    var examQuestions = allQuestions.Where(q => q.ExamId == id).ToList();

        //    // 🟠 إزالة الأسئلة اللي اتشالت
        //    foreach (var oldQuestion in examQuestions.Where(q => !input.QuestionIds.Contains(q.Id)))
        //    {
        //        oldQuestion.ExamId = null;
        //    }

        //    // 🟢 إضافة الأسئلة الجديدة
        //    foreach (var questionId in input.QuestionIds)
        //    {
        //        var question = allQuestions.FirstOrDefault(q => q.Id == questionId);
        //        if (question != null)
        //            question.ExamId = id;
        //    }

        //    // ✅ تحديث جميع الأسئلة مرة واحدة (بدون autoSave لكل واحدة)
        //    await _questionRepository.UpdateManyAsync(allQuestions, autoSave: true);

        //    // ✅ حفظ التغييرات على الامتحان
        //    await _examRepository.UpdateAsync(exam, autoSave: true);
        //}

    }
}
