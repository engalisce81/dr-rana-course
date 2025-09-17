using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Questions
{
    public class QuestionType :AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public int Key { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
