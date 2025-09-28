using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class CreateUpdateQuizStudentDto
    {

        public Guid? LectureId { get; set; }  // this is ref Id 
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public int Score { get; set; }
    }
}
