using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Subjects
{
    public  class Subject : AuditedAggregateRoot<Guid>  
    {
        public string Name { get; set; }
        public ICollection<Courses.Course> Courses { get; set; } = new List<Courses.Course>();
    }
}
