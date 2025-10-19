﻿using AutoMapper;
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
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Response;
using Dev.Acadmy.LookUp;
using Volo.Abp;
using Dev.Acadmy.Chapters;
using Dev.Acadmy.Lectures;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Exams;

namespace Dev.Acadmy.Courses
{
    public class CourseManager : DomainService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly MediaItemManager _mediaItemManager; 
        private readonly IIdentityUserRepository _userRepository;
        private readonly QuestionBankManager _questionBankManager;
        private readonly IRepository<CourseStudent, Guid> _courseStudentRepository;
        private readonly ChapterManager _chapterManager;
        private readonly CourseInfoManager _courseInfoManager;
        private readonly LectureManager _lectureManager;
        private readonly QuizManager _quizManger;
        private readonly QuestionManager _questionManager;
        public CourseManager(QuestionManager questionManager, QuizManager quizManger, LectureManager lectureManager, CourseInfoManager courseInfoManager, ChapterManager chapterManager, IRepository<CourseStudent, Guid> courseStudentRepository, QuestionBankManager questionBankManager, IIdentityUserRepository userRepository, MediaItemManager mediaItemManager, ICurrentUser currentUser, IRepository<Course> courseRepository , IMapper mapper) 
        {
            _questionManager = questionManager;
            _quizManger = quizManger;
            _lectureManager = lectureManager;
            _courseInfoManager = courseInfoManager;
            _chapterManager = chapterManager;
            _courseStudentRepository = courseStudentRepository;
            _questionBankManager = questionBankManager;
            _userRepository = userRepository;
            _mediaItemManager = mediaItemManager;
            _currentUser = currentUser;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        public async Task<ResponseApi<CourseDto>> GetAsync(Guid id)
        {
            var course = await(await _courseRepository.GetQueryableAsync()).Include(x=>x.CourseInfos).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return new ResponseApi<CourseDto> { Data = null, Success = false, Message = "Not found Course" };
            var dto = _mapper.Map<CourseDto>(course);
            var mediaItem = await _mediaItemManager.GetAsync(dto.Id );
            dto.LogoUrl = mediaItem?.Url ?? "";
            foreach(var info in course.CourseInfos) dto.Infos.Add(info.Name);
            
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<CourseDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _courseRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.College).Where(c => c.Name.Contains(search));
            var courses = new List<Course>();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            if (roles.Any(x=>x.Name.ToUpper() ==RoleConsts.Admin.ToUpper() )) courses = await AsyncExecuter.ToListAsync(queryable.Include(x => x.College).Include(x => x.Exams).Include(x => x.QuestionBanks).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else courses = await AsyncExecuter.ToListAsync(queryable.Where(c => c.UserId == _currentUser.GetId()).Include(x => x.College).Include(x=>x.Exams).Include(x=>x.QuestionBanks).Include(x=>x.Subject).Include(x=>x.CourseInfos).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var courseDtos = _mapper.Map<List<CourseDto>>(courses);
            foreach (var courseDto in courseDtos)
            {
                var mediaItem = await _mediaItemManager.GetAsync(courseDto.Id);
                courseDto.LogoUrl = mediaItem?.Url??"";
            }
            return new PagedResultDto<CourseDto>(totalCount, courseDtos);
        }

        public async Task<ResponseApi<CourseDto>> CreateAsync(CreateUpdateCourseDto input)
        {
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var course = _mapper.Map<Course>(input);
            course.UserId = currentUser.Id;
            var collegeId = currentUser.GetProperty<Guid>(SetPropConsts.CollegeId);
            course.CollegeId = collegeId;
            var result = await _courseRepository.InsertAsync(course);
            foreach (var info in input.Infos) await _courseInfoManager.CreateAsync(new CreateUpdateCourseInfoDto { Name = info, CourseId = result.Id });
            await _mediaItemManager.CreateAsync(new CreateUpdateMediaItemDto { Url =input.LogoUrl, RefId = result.Id,IsImage=true});
         //   await _questionBankManager.CreateAsync(new CreateUpdateQuestionBankDto {CreatorId =result.UserId, CourseId = result.Id, Name = $"{result.Name} Question Bank" });
            var dto = _mapper.Map<CourseDto>(result);
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "save succeess" };
        }  

        public async Task<ResponseApi<CourseDto>> UpdateAsync(Guid id, CreateUpdateCourseDto input)
        {
            var courseDB = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (courseDB == null) return new ResponseApi<CourseDto> { Data = null, Success = false, Message = "Not found Course" };
            var course = _mapper.Map(input, courseDB);
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var collegeId = currentUser.GetProperty<Guid>(SetPropConsts.CollegeId);
            course.CollegeId = collegeId;
            var result = await _courseRepository.UpdateAsync(course);
            await _courseInfoManager.DeleteCourseInfoByCourseId(course.Id);
            foreach (var info in input.Infos) await _courseInfoManager.CreateAsync(new CreateUpdateCourseInfoDto { Name = info, CourseId = result.Id });
            await _mediaItemManager.UpdateAsync(id, new CreateUpdateMediaItemDto { Url = input.LogoUrl, RefId = result.Id ,IsImage=true });
            // var questionBank = await _questionBankManager.GetByCourse(id);
            //if(questionBank !=null) await _questionBankManager.UpdateAsync(questionBank.Id, new CreateUpdateQuestionBankDto { CreatorId=result.UserId,CourseId = result.Id, Name = $"{result.Name} Question Bank" });
            var dto = _mapper.Map<CourseDto>(result);
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "update succeess" };
        }
        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var course = await (await _courseRepository.GetQueryableAsync()).Include(x=>x.Chapters).Include(x=>x.CourseInfos).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found Course" };
            var courseStudent =await (await _courseStudentRepository.GetQueryableAsync()).Where(x=>x.CourseId == course.Id).ToListAsync();
            if (courseStudent != null) await _courseStudentRepository.DeleteManyAsync(courseStudent);
           // var questionBank = await _questionBankManager.GetByCourse(id);
           // if (questionBank != null) await _questionBankManager.DeleteAsync(questionBank.Id);
            var chapterIds = course.Chapters.Select(x => x.Id).ToList();
            if (chapterIds.Any()) foreach (var chapter in chapterIds) await _chapterManager.DeleteAsync(chapter);
            var infos = course.CourseInfos.Select(x => x.Id);
            if (infos.Any()) foreach (var info in infos) await _courseInfoManager.DeleteAsync(info);
            await _mediaItemManager.DeleteAsync(id);
            await _courseRepository.DeleteAsync(course);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetCoursesListAsync()
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());

            var queryable = await _courseRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var courses = new List<Course>();
            if(roles.Any(x=>x.Name.ToUpper()==RoleConsts.Admin)) courses = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime));
            else courses = await AsyncExecuter.ToListAsync(queryable.Where(c => c.UserId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime));
            var courseDtos = _mapper.Map<List<LookupDto>>(courses);
            return new PagedResultDto<LookupDto>(totalCount, courseDtos);
        }

        public async Task<PagedResultDto<CourseInfoHomeDto>> GetCoursesInfoListAsync(
    int pageNumber,
    int pageSize,
    string? search,
    bool alreadyJoin,
    Guid collegeId,
    Guid? subjectId,
    Guid? gradelevelId)
        {
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var termId = currentUser.GetProperty<Guid?>(SetPropConsts.TermId);

            if (collegeId == Guid.Empty)
                return new PagedResultDto<CourseInfoHomeDto>(0, new List<CourseInfoHomeDto>());

            var courseStudents = await (await _courseStudentRepository.GetQueryableAsync())
                .Where(x => x.UserId == currentUser.Id)
                .ToListAsync();

            var alreadyJoinCourses = courseStudents
                .Where(x => x.IsSubscibe)
                .Select(x => x.CourseId)
                .ToList();

            var alreadyRequestCourses = courseStudents
                .Select(x => x.CourseId)
                .ToList();

            var queryable = await _courseRepository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                queryable = queryable
                    .Include(c => c.Subject)
                        .ThenInclude(x => x.GradeLevel)
                    .Where(c =>
                        c.Name.Contains(search) ||
                        c.Description.Contains(search) ||
                        c.Subject.Name.Contains(search));
            }

            // ✅ تعديل هنا
            if (alreadyJoin)
            {
                // نجيب فقط الكورسات اللي الطالب مشترك فيها في أي كلية
                queryable = queryable.Where(c => alreadyJoinCourses.Contains(c.Id));
            }
            else
            {
                // نحافظ على الفلاتر العادية حسب الكلية والمادة والمستوى
                queryable = queryable.Where(c =>
                    c.CollegeId == collegeId &&
                    (!subjectId.HasValue || c.SubjectId == subjectId.Value) &&
                    (!termId.HasValue || c.Subject.TermId == termId.Value) &&
                    (!gradelevelId.HasValue || c.Subject.GradeLevelId == gradelevelId.Value));
            }

            var totalCount = await queryable.CountAsync();

            var courses = await queryable
                .Include(c => c.User)
                .Include(c => c.College)
                .Include(c => c.Chapters)
                .OrderByDescending(c => c.CreationTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mediaItems = new Dictionary<Guid, MediaItem>();
            foreach (var course in courses)
            {
                var media = await _mediaItemManager.GetAsync(course.Id);
                if (media != null)
                {
                    mediaItems[course.Id] = media;
                }
            }

            var courseDtos = courses.Select(course => new CourseInfoHomeDto
            {
                Id = course.Id,
                Name = course.Name,
                IsPdf = course.IsPdf,
                PdfUrl = course.PdfUrl,
                Description = course.Description,
                Price = course.Price,
                LogoUrl = mediaItems.TryGetValue(course.Id, out var media)
                    ? media.Url
                    : "",
                UserId = course.UserId,
                UserName = course.User?.Name ?? "",
                CollegeId = course.CollegeId,
                CollegeName = course.College?.Name ?? "",
                AlreadyJoin = alreadyJoinCourses.Contains(course.Id),
                AlreadyRequest = alreadyRequestCourses.Contains(course.Id),
                SubjectId = course.Subject?.Id,
                SubjectName = course.Subject?.Name ?? "",
                ChapterCount = course.Chapters.Count,
                DurationInWeeks = course.DurationInDays / 7,
                GradelevelId = course.Subject?.GradeLevelId ?? null,
                GradelevelName = course.Subject?.GradeLevel?.Name ?? string.Empty,
                IntroductionVideoUrl = course.IntroductionVideoUrl,
            }).ToList();

            return new PagedResultDto<CourseInfoHomeDto>(totalCount, courseDtos);
        }

        public async Task<ResponseApi<CourseInfoHomeDto>> GetCoursesInfoAsync( Guid courseId)
        {
            var currentUser = await  _userRepository.GetAsync(_currentUser.GetId());
            var courseStudents = await (await _courseStudentRepository.GetQueryableAsync()).Where(x => x.UserId == currentUser.Id).ToListAsync();
            var alreadyJoinCourses = courseStudents.Where(x => x.IsSubscibe && x.UserId == currentUser.Id).Select(x => x.CourseId).ToList();
            var alreadyRequestCourses = courseStudents.Select(x => x.CourseId).ToList();
            var queryable = await _courseRepository.GetQueryableAsync();
            var course = await queryable.Include(c => c.User).Include(x => x.Subject).Include(x=>x.CourseInfos).Include(c => c.College).Include(c => c.Chapters).OrderByDescending(c => c.CreationTime).FirstOrDefaultAsync(x=>x.Id==courseId);
            if (course == null) { throw new UserFriendlyException("Course Not Found"); }
            var media = await _mediaItemManager.GetAsync(courseId); 
            var courseDto =  new CourseInfoHomeDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                LogoUrl = media?.Url?? "",
                UserId = course.UserId,
                UserName = course.User?.Name ?? "",
                CollegeId = course.CollegeId,
                CollegeName = course.College?.Name ?? "",
                AlreadyJoin = alreadyJoinCourses.Contains(course.Id),
                AlreadyRequest = alreadyRequestCourses.Contains(course.Id),
                SubjectId = course.Subject?.Id,
                SubjectName = course.Subject?.Name ?? "",
                ChapterCount = course.Chapters.Count,
                DurationInWeeks = course.DurationInDays / 7,
                IntroductionVideoUrl = course.IntroductionVideoUrl,
                Infos = course.CourseInfos.Select(x=>x.Name).ToList()
            };
            return new ResponseApi<CourseInfoHomeDto>() { Data= courseDto ,Success=true , Message="Find Course Success"};
        }

        public async Task<PagedResultDto<LookupDto>> GetMyCoursesLookUpAsync()
        {
            // هات الـ User الحالي
            var currentUserId = _currentUser.GetId();

            // هات كل الكورسات اللي انت الـ CreatorId فيها
            var queryable = await _courseRepository.GetQueryableAsync();
            var myCourses = await queryable
                .Where(c => c.UserId == currentUserId)
                .ToListAsync();
            if (!myCourses.Any()) return new PagedResultDto<LookupDto>(0,new List<LookupDto>() );
            // اعمل Map للـ DTO
            var courseDtos = _mapper.Map<List<LookupDto>>(myCourses);
            return new PagedResultDto<LookupDto>( courseDtos.Count(),courseDtos);
        }

        public async Task<Guid> DuplicateCourseAsync(Guid courseId)
        {
            // 🟢 1. تحميل الكورس الأصلي بكامل العلاقات
            var course = await (await _courseRepository.GetQueryableAsync())
                .Include(x => x.Chapters)
                    .ThenInclude(ch => ch.Lectures)
                        .ThenInclude(l => l.Quizzes)
                            .ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                throw new UserFriendlyException("Course not found");

            // 🟢 2. إنشاء نسخة جديدة من الكورس
            var newCourse = new Course
            {
                Name = course.Name + " (Copy)",
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Rating = 0,
                UserId = _currentUser.GetId(),
                CollegeId = course.CollegeId,
                SubjectId = course.SubjectId,
                IsActive = true,
                IsLifetime = course.IsLifetime,
                DurationInDays = course.DurationInDays,
                IsPdf = course.IsPdf,
                PdfUrl= course.PdfUrl,
                IntroductionVideoUrl= course.IntroductionVideoUrl,
            };
            await _courseRepository.InsertAsync(newCourse, autoSave: true);
            await _mediaItemManager.CreateAsync(new CreateUpdateMediaItemDto { Url = (await _mediaItemManager.GetAsync(course.Id))?.Url ?? "", RefId = newCourse.Id, IsImage = true });
           // await _questionBankManager.CreateAsync(new CreateUpdateQuestionBankDto { CreatorId = newCourse.UserId, CourseId = newCourse.Id, Name = $"{newCourse.Name} Question Bank (Copy)" });
            // انسخ CourseInfos
            foreach (var info in course.CourseInfos)
            {
                await _courseInfoManager.CreateAsync(new CreateUpdateCourseInfoDto { Name = info.Name, CourseId = newCourse.Id });
            }


            // 🟢 3. نسخ الفصول (Chapters)
            foreach (var chapter in course.Chapters)
            {
                var newChapter = new CreateUpdateChapterDto
                {
                    Name = chapter.Name,
                    CourseId = newCourse.Id
                };
                var chapterDto = await _chapterManager.CreateAsync(newChapter);

                // 🟢 4. نسخ المحاضرات (Lectures)
                foreach (var lecture in chapter.Lectures)
                {
                    var newLecture = new CreateUpdateLectureDto
                    {
                        Title = lecture.Title,
                        Content = lecture.Content,
                        VideoUrl = lecture.VideoUrl,
                        ChapterId = chapterDto.Data.Id,
                        IsVisible = lecture.IsVisible,
                        QuizTryCount = lecture.QuizTryCount
                    };
                    var lecDto  = await _lectureManager.CreateAsync(newLecture);

                    // 🟢 5. نسخ الكويزات (Quizzes)
                    foreach (var quiz in lecture.Quizzes)
                    {
                        var newQuiz = new CreateUpdateQuizDto
                        {
                            Title = quiz.Title,
                            Description = quiz.Description,
                            QuizTime = quiz.QuizTime,
                            QuizTryCount = quiz.QuizTryCount,
                            LectureId = lecDto.Data.Id
                        };
                        var quizDto = await _quizManger.CreateAsync(newQuiz);

                        // 🟢 6. نسخ الأسئلة (Questions)
                        foreach (var question in quiz.Questions)
                        {
                            var newQuestion = new CreateUpdateQuestionDto
                            {
                                Title = question.Title,
                                QuizId = quizDto.Data.Id,
                                QuestionTypeId = question.QuestionTypeId,
                                QuestionBankId = question.QuestionBankId,
                                Answers = question.QuestionAnswers.Select(a => new CreateUpdateQuestionAnswerDto
                                {
                                    Answer = a.Answer,
                                    IsCorrect = a.IsCorrect,
                                }).ToList(),
                                Score=question.Score
                            };
                            await _questionManager.CreateAsync(newQuestion);
                        }
                    }
                }
            }

            return newCourse.Id;
        }
    }
}
