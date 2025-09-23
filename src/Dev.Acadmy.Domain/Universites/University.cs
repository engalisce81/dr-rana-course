using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Universites
{
    public class University :AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public ICollection<College> Colleges { get; set; } = new List<College>();
    }
}
