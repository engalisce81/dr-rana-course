using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Universites
{
    public class GradeLevel :AuditedAggregateRoot<Guid>
    {
        public string Name {  get; set; }
        public Guid CollegeId { get; set; }
        public College College { get; set; }
        public ICollection<Subject> Subjectls { get; set; } = new List<Subject>();
    }
}
