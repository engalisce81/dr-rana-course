using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Exams
{
    public class ExamQuestion:AuditedAggregateRoot<Guid>
    {
        public Guid ExamId { get; set; }
        public Guid QuestionId { get; set; }
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
