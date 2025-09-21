using AutoMapper;
using Dev.Acadmy.Chapters;
using Dev.Acadmy.Colleges;
using Dev.Acadmy.Courses;
using Dev.Acadmy.Lectures;
using Dev.Acadmy.LookUp;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Quizzes;
using Dev.Acadmy.Subjects;

namespace Dev.Acadmy;

public class AcadmyApplicationAutoMapperProfile : Profile
{
    public AcadmyApplicationAutoMapperProfile()
    {
        CreateMap<College, CollegeDto>();
        CreateMap<CreateUpdateCollegeDto, College>();

        // Course
        CreateMap<Courses.Course, CourseDto>();
        CreateMap<CreateUpdateCourseDto, Courses.Course>();

        // Chapter
        CreateMap<Chapter, ChapterDto>();
        CreateMap<CreateUpdateChapterDto, Chapter>();

        // CourseStudent
        CreateMap<CourseStudent, CourseStudentDto>();
        CreateMap<CreateUpdateCourseStudentDto, CourseStudent>();

        // Lecture
        CreateMap<Lecture, LectureDto>();
        CreateMap<CreateUpdateLectureDto, Lecture>();

        // MediaItem
        CreateMap<MediaItem, MediaItemDto>();
        CreateMap<CreateUpdateMediaItemDto, MediaItem>();

        // Question
        CreateMap<Question, QuestionDto>();
        CreateMap<CreateUpdateQuestionDto, Question>();

        // QuestionAnswer
        CreateMap<Questions.QuestionAnswer, QuestionAnswerDto>();
        CreateMap<CreateUpdateQuestionAnswerDto, Questions.QuestionAnswer>();

        // QuestionBank
        CreateMap<QuestionBank, QuestionBankDto>();
        CreateMap<CreateUpdateQuestionBankDto, QuestionBank>();

        // QuestionType
        CreateMap<QuestionType, QuestionTypeDto>();
        CreateMap<CreateUpdateQuestionTypeDto, QuestionType>();

        // Quiz
        CreateMap<Quiz, QuizDto>();
        CreateMap<CreateUpdateQuizDto, Quiz>();

        // QuizStudent
        CreateMap<QuizStudent, QuizStudentDto>();
        CreateMap<CreateUpdateQuizStudentDto, QuizStudent>();

        CreateMap<Subject, SubjectDto>();
        CreateMap<CreateUpdateSubjectDto, Subject>();

        CreateMap<CourseInfo, CourseInfoDto>();
        CreateMap<CreateUpdateCourseInfoDto, CourseInfo>();

        CreateMap<LookupDto, QuestionBank>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name)); ;
        CreateMap<LookupDto, QuestionType>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<LookupDto, Quiz>().ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        CreateMap<LookupDto, Question>().ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        CreateMap<LookupDto, College>();
        CreateMap<LookupDto, Courses.Course>();
        CreateMap<LookupDto, Chapter>();
        CreateMap<LookupDto, Subject>();

        CreateMap<QuestionBank, LookupDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<QuestionType, LookupDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<Quiz, LookupDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));
        CreateMap<Question, LookupDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title));
        CreateMap<College, LookupDto>();
        CreateMap<Courses.Course, LookupDto>();
        CreateMap<Chapter, LookupDto>();
        CreateMap< Subject ,LookupDto>();
    }
}
