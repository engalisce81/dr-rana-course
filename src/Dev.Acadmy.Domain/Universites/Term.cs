using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Universites
{
    public class Term:AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
