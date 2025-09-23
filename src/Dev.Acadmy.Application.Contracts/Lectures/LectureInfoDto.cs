using Dev.Acadmy.Quizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Lectures
{
    public class LectureInfoDto
    {
        public Guid LectureId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public QuizInfoDto Quiz { get; set; }
    }
}
