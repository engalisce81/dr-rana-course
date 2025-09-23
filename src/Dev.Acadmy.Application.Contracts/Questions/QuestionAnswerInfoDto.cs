using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class QuestionAnswerInfoDto
    {
        public Guid QuestionId { get; set; }
        public string? TextAnswer { get; set; }  
        public Guid? SelectedAnswerId { get; set; }
    }
}
