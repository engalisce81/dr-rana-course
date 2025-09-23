using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class QuizAnswerDto
    {
        public Guid QuizId { get; set; }
        public List<QuestionAnswerInfoDto> Answers { get; set; } = new();
    }
}
