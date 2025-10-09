using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Exams
{
    public class Exam:AuditedAggregateRoot<Guid>
    {
        public string Name {  get; set; }
        public int TimeExam { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
