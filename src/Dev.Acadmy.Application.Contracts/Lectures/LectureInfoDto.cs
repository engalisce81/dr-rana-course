using Dev.Acadmy.Quizzes;
using System;
namespace Dev.Acadmy.Lectures
{
    public class LectureInfoDto
    {
        public Guid LectureId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public string? PdfUrl { get; set; }

        public QuizInfoDto Quiz { get; set; }
    }
}
