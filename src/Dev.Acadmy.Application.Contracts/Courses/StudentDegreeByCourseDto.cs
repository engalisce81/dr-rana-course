using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Courses
{
    public class StudentDegreeByCourseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public List<StudentQuizDto> Quizzes { get; set; } = new();
    }
}
