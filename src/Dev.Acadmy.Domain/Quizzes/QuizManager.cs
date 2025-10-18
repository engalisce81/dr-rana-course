using AutoMapper;
using Dev.Acadmy.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Users;
using Volo.Abp.Identity;
using static Dev.Acadmy.Permissions.AcadmyPermissions;
using Dev.Acadmy.Lectures;

namespace Dev.Acadmy.Quizzes
{
    public class QuizManager :DomainService
    {
        private readonly IRepository<Quiz ,Guid> _quizRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<QuizStudent> _quizStudentRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IRepository<LectureStudent, Guid> _lectureStudentRepository;
        private readonly IRepository<Lecture, Guid> _lectureRepository;
        public QuizManager(IRepository<Lecture, Guid> lectureRepository, IRepository<LectureStudent, Guid> lectureStudentRepository, IIdentityUserRepository userRepository, ICurrentUser currentUser, IRepository<QuizStudent> quizStudentRepository, IMapper mapper, IRepository<Quiz,Guid> quizRepository)
        {
            _lectureRepository = lectureRepository;
            _lectureStudentRepository = lectureStudentRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _quizRepository = quizRepository;
            _mapper = mapper;
            _quizStudentRepository = quizStudentRepository;
        }

        public async Task<ResponseApi<QuizDto>> GetAsync(Guid id)
        {
            var quiz = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new ResponseApi<QuizDto> { Data = null, Success = false, Message = "Not found quiz" };
            var dto = _mapper.Map<QuizDto>(quiz);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<List<Quiz>> GetQuizesByLectureId(Guid lectureId) => await ( await _quizRepository.GetQueryableAsync()).Where(x=>x.LectureId == lectureId).ToListAsync();
        
        public async Task DeletQuizesByLectureId(Guid lectureId)
        {
            var quizes = await (await _quizRepository.GetQueryableAsync()).Where(x => x.LectureId == lectureId).ToListAsync();
            foreach(var quiz in quizes)
            {
                var studentQuiz = await (await _quizStudentRepository.GetQueryableAsync()).Where(x => x.QuizId == quiz.Id).ToListAsync();
                await _quizStudentRepository.DeleteManyAsync(studentQuiz);
                await _quizRepository.DeleteAsync(quiz);
            }
        }

        public async Task<PagedResultDto<QuizDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var queryable = await _quizRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Where(c => c.Description.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var quizs = await AsyncExecuter.ToListAsync(queryable.OrderBy(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var quizDtos = _mapper.Map<List<QuizDto>>(quizs);
            return new PagedResultDto<QuizDto>(totalCount, quizDtos);
        }

        public async Task<ResponseApi<QuizDto>> CreateAsync(CreateUpdateQuizDto input)
        {
            var quiz = _mapper.Map<Quiz>(input);
            var result = await _quizRepository.InsertAsync(quiz ,autoSave:true);
            var dto = _mapper.Map<QuizDto>(result);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<QuizDto>> UpdateAsync(Guid id, CreateUpdateQuizDto input)
        {
            var quizDB = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quizDB == null) return new ResponseApi<QuizDto> { Data = null, Success = false, Message = "Not found quiz" };
            var quiz = _mapper.Map(input, quizDB);
            var result = await _quizRepository.UpdateAsync(quiz,autoSave:true);
            var dto = _mapper.Map<QuizDto>(result);
            return new ResponseApi<QuizDto> { Data = dto, Success = true, Message = "update succeess" };
        }

        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var quiz = await _quizRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found quiz" };
            await _quizRepository.DeleteAsync(quiz ,autoSave:true);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<ResponseApi<QuizResultDto>> CorrectQuizAsync(QuizAnswerDto input)
        {
            var userId = _currentUser.GetId();
            var quiz = await (await _quizRepository.GetQueryableAsync())
                .Include(q => q.Questions)
                    .ThenInclude(q => q.QuestionAnswers)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.QuestionType)
                .Include(x=>x.Lecture)
                .FirstOrDefaultAsync(q => q.Id == input.QuizId);

            if (quiz == null)
                throw new UserFriendlyException("Quiz not found");

            var alreadyAnswered = await _quizStudentRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.QuizId == input.QuizId);
            if (alreadyAnswered != null)
                throw new UserFriendlyException("You have already submitted this quiz. You cannot attempt it again.");

            double totalScore = 0;
            double studentScore = 0;

            var quizStudent = new QuizStudent
            {
                LectureId = quiz.LectureId,
                UserId = userId,
                QuizId = quiz.Id
            };

            foreach (var question in quiz.Questions)
            {
                var studentAnswer = input.Answers.FirstOrDefault(a => a.QuestionId == question.Id);
                if (studentAnswer == null)
                    continue;

                bool isCorrect = false;
                double obtained = 0;

                switch ((QuestionTypeEnum)question.QuestionType.Key)
                {
                    case QuestionTypeEnum.MCQ:
                    case QuestionTypeEnum.TrueOrFalse:
                        if (studentAnswer.SelectedAnswerId != null)
                        {
                            var correctAnswer = question.QuestionAnswers.FirstOrDefault(a => a.IsCorrect);
                            if (correctAnswer != null && correctAnswer.Id == studentAnswer.SelectedAnswerId)
                            {
                                isCorrect = true;
                                obtained = question.Score;
                                studentScore += question.Score;
                            }
                        }
                        break;

                    case QuestionTypeEnum.ShortAnswer:
                        if (!string.IsNullOrWhiteSpace(studentAnswer.TextAnswer))
                        {
                            var keywords = question.QuestionAnswers.Select(a => a.Answer.ToLower()).ToList();
                            var studentText = studentAnswer.TextAnswer.ToLower();

                            int matched = keywords.Count(k => studentText.Contains(k));
                            if (matched > 0)
                            {
                                double ratio = (double)matched / keywords.Count;
                                obtained = question.Score * ratio;
                                studentScore += obtained;
                                isCorrect = ratio >= 0.8; // مثلاً اعتبرها صحيحة لو أكثر من 80% من الكلمات متطابقة
                            }
                        }
                        break;

                    case QuestionTypeEnum.CompleteAnswer:
                        if (!string.IsNullOrWhiteSpace(studentAnswer.TextAnswer))
                        {
                            var correctAnswer = question.QuestionAnswers.FirstOrDefault(a => a.IsCorrect);
                            if (correctAnswer != null &&
                                string.Equals(studentAnswer.TextAnswer.Trim(), correctAnswer.Answer.Trim(), StringComparison.OrdinalIgnoreCase))
                            {
                                obtained = question.Score;
                                studentScore += question.Score;
                                isCorrect = true;
                            }
                        }
                        break;
                }

                totalScore += question.Score;
                quizStudent.Answers.Add(new QuizStudentAnswer
                {
                    QuestionId = question.Id,
                    SelectedAnswerId = studentAnswer.SelectedAnswerId,
                    TextAnswer = studentAnswer.TextAnswer,
                    IsCorrect = isCorrect,
                    ScoreObtained = obtained
                });
            }

            quizStudent.Score = (int)studentScore;
            await _quizStudentRepository.InsertAsync(quizStudent, autoSave: true);

            return new ResponseApi<QuizResultDto>
            {
                Data = new QuizResultDto
                {
                    QuizId = quiz.Id,
                    TotalScore = totalScore,
                    StudentScore = studentScore
                },
                Success = true,
                Message = "Quiz corrected successfully"
            };
        }

        public async Task<ResponseApi<LectureQuizResultDto>> GetLectureQuizResultsAsync(Guid lectureId)
        {
            var userId = _currentUser.GetId();

            // نحمل المحاضرة بالكويزات التابعة لها
            var lecture = await (await _lectureRepository.GetQueryableAsync())
                .Include(l => l.Quizzes)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.QuestionAnswers)
                .FirstOrDefaultAsync(l => l.Id == lectureId);

            if (lecture == null)
                throw new UserFriendlyException("Lecture not found");

            // نحمل نتائج الطالب في الكويزات دي
            var quizStudents = await (await _quizStudentRepository.GetQueryableAsync())
                .Include(qs => qs.Answers)
                .Where(qs => qs.UserId == userId && qs.LectureId == lectureId)
                .ToListAsync();

            var lectureResult = new LectureQuizResultDto
            {
                LectureId = lecture.Id,
                LectureTitle = lecture.Title, // أو Title حسب الموديل
            };

            foreach (var quiz in lecture.Quizzes)
            {
                var quizStudent = quizStudents.FirstOrDefault(x => x.QuizId == quiz.Id);
                if (quizStudent == null) continue; // الطالب لسه ما حلش الكويز ده

                double totalScore = quiz.Questions.Sum(q => q.Score);

                var quizResult = new QuizResultDetailDto
                {
                    QuizId = quiz.Id,
                    QuizTitle = quiz.Title,
                    StudentScore = quizStudent.Score,
                    TotalScore = totalScore,
                };

                foreach (var question in quiz.Questions)
                {
                    var studentAnswer = quizStudent.Answers.FirstOrDefault(a => a.QuestionId == question.Id);

                    var correctAnswer = question.QuestionAnswers.FirstOrDefault(a => a.IsCorrect);
                    string correctAnswerText = correctAnswer?.Answer;

                    string studentAnswerText = studentAnswer?.TextAnswer;
                    if (studentAnswer?.SelectedAnswerId != null)
                    {
                        var selected = question.QuestionAnswers.FirstOrDefault(a => a.Id == studentAnswer.SelectedAnswerId);
                        studentAnswerText = selected?.Answer;
                    }

                    quizResult.Questions.Add(new QuestionResultDto
                    {
                        QuestionId = question.Id,
                        QuestionText = question.Title, // أو Text حسب الموديل
                        StudentAnswer = studentAnswerText,
                        CorrectAnswer = correctAnswerText,
                        IsCorrect = studentAnswer?.IsCorrect ?? false,
                        ScoreObtained = studentAnswer?.ScoreObtained ?? 0,
                        ScoreTotal = question.Score
                    });
                }

                lectureResult.Quizzes.Add(quizResult);
            }

            return new ResponseApi<LectureQuizResultDto>
            {
                Data = lectureResult,
                Success = true,
                Message = "Lecture quiz results retrieved successfully"
            };
        }


        public async Task<ResponseApi<QuizStudentDto>> MarkQuizAsync(Guid quizId, int score)
        {
            var userId = _currentUser.GetId();
            var quiz = await _quizRepository.GetAsync(quizId);

            if (quiz == null)
            {
                return new ResponseApi<QuizStudentDto>
                {
                    Success = false,
                    Message = "Quiz not found",
                    Data = null
                };
            }

            // هات المحاضرة
            var lectureId = quiz.LectureId;
            if (lectureId == null)
            {
                return new ResponseApi<QuizStudentDto>
                {
                    Success = false,
                    Message = "Quiz is not linked to a lecture",
                    Data = null
                };
            }
            var lecture = await _lectureRepository.FirstOrDefaultAsync(x => x.Id == lectureId);
            // هات LectureStudent
            var lectureStudent = await _lectureStudentRepository.FirstOrDefaultAsync(x =>
                x.LectureId == lectureId && x.UserId == userId);

            if (lectureStudent == null)
            {
                lectureStudent = new LectureStudent
                {
                    LectureId = lectureId.Value,
                    UserId = userId,
                    AttemptsUsed = 0,
                    MaxAttempts =  lecture?.QuizTryCount??0
                };
                await _lectureStudentRepository.InsertAsync(lectureStudent);
            }

            if (lectureStudent.IsCompleted)
            {
                return new ResponseApi<QuizStudentDto>
                {
                    Success = false,
                    Message = "لقد استهلكت كل المحاولات المتاحة لهذه المحاضرة",
                    Data = null
                };
            }

            // عدل المحاولات
            lectureStudent.AttemptsUsed++;
            await _lectureStudentRepository.UpdateAsync(lectureStudent);

            // سجل QuizStudent
            var quizStudent = new QuizStudent
            {
                LectureId = lectureId,
                UserId = userId,
                QuizId = quizId,
                Score = score
            };
            await _quizStudentRepository.InsertAsync(quizStudent);

            // رجع الـ DTO
            var dto = new QuizStudentDto
            {
                LectureId = lectureId.Value,
                QuizId = quizId,
                UserId = userId,
                Score = score
            };

            return new ResponseApi<QuizStudentDto>
            {
                Success = true,
                Message = "تم تسجيل نتيجتك بنجاح",
                Data = dto
            };
        }


    }
}
