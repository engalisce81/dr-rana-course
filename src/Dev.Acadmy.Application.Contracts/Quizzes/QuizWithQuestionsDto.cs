using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class QuizWithQuestionsDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<QuestionWithAnswersDto> Questions { get; set; } = new();
    }
}
