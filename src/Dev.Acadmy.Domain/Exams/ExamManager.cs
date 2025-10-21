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
        private readonly IRepository<ExamQuestion, Guid> _examQuestionRepository;
        public ExamManager(IRepository<ExamQuestion, Guid> examQuestionRepository, IRepository<ExamQuestionBank, Guid> examQuestionBankRepository, ICurrentUser currentUser , IIdentityUserRepository userRepository, IRepository<QuestionBank, Guid> questionBankRepository, IMapper mapper, IRepository<Exam,Guid> examRepository , IRepository<Question, Guid> questionRepository)
        {
            _examQuestionRepository = examQuestionRepository;
            _examQuestionBankRepository = examQuestionBankRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            _questionBankRepository = questionBankRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
            _examRepository = examRepository;
        }

        //public async Task<ResponseApi<ExamDto>> GetAsync(Guid id)
        //{
        //    // نجيب examQuestionBanks المرتبطة بالامتحان
        //    var examQuestionBanks = await (await _examQuestionBankRepository.GetQueryableAsync())
        //        .Where(x => x.ExamId == id)
        //        .Include(x => x.Exam)
        //        .Include(x => x.QuestionBank)
        //            .ThenInclude(qb => qb.Questions)
        //                .ThenInclude(q => q.QuestionAnswers)
        //        .ToListAsync();

        //    // لو مفيش حاجة
        //    if (!examQuestionBanks.Any())
        //        return new ResponseApi<ExamDto>
        //        {
        //            Success = false,
        //            Message = "Exam not found",
        //            Data = null
        //        };

        //    // ناخد أول exam كمرجع (كلهم لنفس الامتحان)
        //    var examEntity = examQuestionBanks.First().Exam;

        //    // نبني ExamDto واحد فقط
        //    var dto = new ExamDto
        //    {
        //        Id = examEntity.Id,
        //        Name = examEntity.Name,
        //        TimeExam = examEntity.TimeExam,
        //        IsActive = examEntity.IsActive,
        //        ExamQuestions = examQuestionBanks
        //            .SelectMany(eq => eq.QuestionBank.Questions)
        //            .Select(q => new ExamQuestions
        //            {
        //                Id = q.Id,
        //                Tittle = q.Title,
        //                QuestionType = q.QuestionType.Name,
        //                QuestionAnswers = q.QuestionAnswers.Select(qa => new ExamQuestionAnswer
        //                {
        //                    AnswerId = qa.Id,
        //                    Answer = qa.Answer,
        //                    IsSelected = qa.IsCorrect
        //                }).ToList()
        //            })
        //            .ToList()
        //    };

        //    return new ResponseApi<ExamDto>
        //    {
        //        Data = dto,
        //        Success = true,
        //        Message = "Exam loaded successfully"
        //    };
        //}


        public async Task<ResponseApi<ExamDto>> GetAsync(Guid id)
        {
            var exam = await _examRepository.GetAsync(id);
            if (exam == null) return new ResponseApi<ExamDto> { Data = null, Success = false, Message = "Not found exam" };
            var dto = _mapper.Map<ExamDto>(exam);
            return new ResponseApi<ExamDto> { Data = dto, Success = true, Message = "load succeess" };
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
           // await CreateRelation(result, input);
            var dto = _mapper.Map<ExamDto>(result);
            return new ResponseApi<ExamDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<ExamDto>> UpdateAsync(Guid id, CreateUpdateExamDto input)
        {
            var examDB = await _examRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (examDB == null) return new ResponseApi<ExamDto> { Data = null, Success = false, Message = "Not found exam" };
            var exam = _mapper.Map(input, examDB);
            var result = await _examRepository.UpdateAsync(exam);
           // await DeleteRelation(id);
           // await CreateRelation(result, input);
            var dto = _mapper.Map<ExamDto>(result);
            return new ResponseApi<ExamDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var exam = await _examRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (exam == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found exam" };
         //   await DeleteRelation(id);
            await _examRepository.DeleteAsync(exam);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }


        public async Task<PagedResultDto<ExamQuestionsDto>> GetQuestionsFromBankAsync(List<Guid> bankIds, Guid examId)
        {
            // ✅ تحميل بنوك الأسئلة مع الأسئلة والإجابات وأنواع الأسئلة
            var banksQuery = await _questionBankRepository.GetQueryableAsync();
            var banks = await banksQuery
                .Where(x => bankIds.Contains(x.Id))
                .Include(x => x.Questions)
                    .ThenInclude(q => q.QuestionType)
                .Include(x => x.Questions)
                    .ThenInclude(q => q.QuestionAnswers)
                .ToListAsync();

            var questions = new List<ExamQuestionsDto>();

            // ✅ تحميل الأسئلة المرتبطة بالامتحان مرة واحدة (بدلاً من استعلام داخل اللوب)
            var examQuestionIds = await (await _examQuestionRepository.GetQueryableAsync())
                .Where(x => x.ExamId == examId)
                .Select(x => x.QuestionId)
                .ToListAsync();

            foreach (var bank in banks)
            {
                foreach (var question in bank.Questions)
                {
                    var examQuestion = new ExamQuestionsDto
                    {
                        Id = question.Id,
                        Tittle = question.Title,
                        QuestionType = question.QuestionType?.Name,
                        // ✅ التأكد إذا كانت السؤال مرتبط بالامتحان
                        IsSelected = examQuestionIds.Contains(question.Id),
                        QuestionAnswers = question.QuestionAnswers.Select(qa => new ExamQuestionAnswerDto
                        {
                            AnswerId = qa.Id,
                            Answer = qa.Answer,
                            IsSelected = qa.IsCorrect
                        }).ToList()
                    };

                    questions.Add(examQuestion);
                }
            }

            // ✅ ترجيع النتيجة داخل PagedResultDto (بدون pagination حالياً، يمكنك إضافته لاحقاً)
            return new PagedResultDto<ExamQuestionsDto>(questions.Count, questions);
        }

        public async Task AddQuestionToExam(CreateUpdateExamQuestionDto input)
        {
            var examBanks = await (await _examQuestionBankRepository.GetQueryableAsync()).Where(x => input.QuestionBankIds.Contains(x.QuestionBankId) && x.ExamId == input.ExamId).ToListAsync();
            var examQuestions = await (await _examQuestionRepository.GetQueryableAsync()).Where(x => input.QuestionIds.Contains(x.QuestionId) && x.ExamId == input.ExamId).ToListAsync();
            if (examBanks.Any()) await _examQuestionBankRepository.DeleteManyAsync(examBanks);
            if (examQuestions.Any()) await _examQuestionRepository.DeleteManyAsync(examQuestions);
            foreach (var bankId in input.QuestionBankIds)
            {
                var examQuestionBank = new ExamQuestionBank
                {
                    ExamId = input.ExamId,
                    QuestionBankId = bankId
                };
                await _examQuestionBankRepository.InsertAsync(examQuestionBank ,autoSave:true);
            }
            foreach (var questionId in input.QuestionIds)
            {
                var examQuestion = new ExamQuestion
                {
                    ExamId = input.ExamId,
                    QuestionId = questionId
                };
                await _examQuestionRepository.InsertAsync(examQuestion ,autoSave:true);
            }
        }
        

        
        
    }
}
