using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Courses
{
    public class StudentQuizDto
    {
        public string QuizName { get; set; }
        public string LectureName { get; set; }
        public double QuizScore { get; set; }
        public int TryCount { get; set; }
    }

}
