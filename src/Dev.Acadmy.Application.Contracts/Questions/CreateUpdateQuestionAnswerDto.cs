using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class CreateUpdateQuestionAnswerDto
    {
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
    }
}
