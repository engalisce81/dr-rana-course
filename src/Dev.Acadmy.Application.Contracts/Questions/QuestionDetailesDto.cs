using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class QuestionDetailesDto
    {
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public string QuestionType { get; set; }
        public int QuestionTypeKey { get; set; }
        public string LogoUrl { get; set; }
        public List<QuestionAnswerDetailesDto> Answers { get; set; }
    }
}
