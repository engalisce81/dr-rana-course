using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Questions
{
    public class QuestionDto :EntityDto<Guid>
    {
        public string Title { get; set; }
        public Guid QuestionTypeId { get; set; }
        public string QuestionTypeName { get; set; }
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public Guid QuestionBankId { get; set; }
        public string QuestionBankName { get; set; }
        public int Score { get; set; }
        public ICollection<QuestionAnswerDto> Answers { get; set; }= new List<QuestionAnswerDto>();
    }
}
