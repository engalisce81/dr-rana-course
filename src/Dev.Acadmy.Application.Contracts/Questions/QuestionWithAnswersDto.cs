using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class QuestionWithAnswersDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public Guid QuestionTypeId { get; set; }   
        public string QuestionTypeName { get; set; } 
        public List<QuestionAnswerPanelDto> Answers { get; set; } = new();
    }
}
