using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class QuestionBankWithQuestionsDto
    {
        public Guid BankId { get; set; }
        public string BankName { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }

    public class QuestionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string QuestionType { get; set; }
        public List<AnswerDto> CorrectAnswers { get; set; } = new();
    }

    public class AnswerDto
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
    }

}
