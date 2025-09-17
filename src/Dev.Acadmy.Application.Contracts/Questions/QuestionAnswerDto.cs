using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Questions
{
    public class QuestionAnswerDto:EntityDto<Guid>
    {
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
        public string QuestionTitle { get; set; }
    }
}
