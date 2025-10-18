using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Exams
{
    public class ExamQuestionBank:AuditedAggregateRoot<Guid>
    {
        public Guid ExamId { get; set; }
        public Guid QuestionBankId { get; set; }
        [ForeignKey(nameof(ExamId))]
        public Exam Exam { get; set; }
        [ForeignKey(nameof(QuestionBankId))]
        public QuestionBank QuestionBank { get; set; }
    }
}
