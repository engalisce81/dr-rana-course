using System;
using System.Collections.Generic;


namespace Dev.Acadmy.Exams
{
    public class CreateUpdateExamQuestionDto
    {
        public Guid ExamId { get; set; }
        public List<Guid> QuestionIds { get; set; }
        public List<Guid> QuestionBankIds { get; set; }
    }
}
