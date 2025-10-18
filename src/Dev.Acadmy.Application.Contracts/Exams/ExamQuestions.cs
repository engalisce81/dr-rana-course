using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Exams
{
    public class ExamQuestions
    {
        public Guid Id { get; set; }    
        public string Tittle { get; set; }
        public string QuestionType { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<ExamQuestionAnswer> QuestionAnswers { get; set; } = new List<ExamQuestionAnswer>();
    }
}
