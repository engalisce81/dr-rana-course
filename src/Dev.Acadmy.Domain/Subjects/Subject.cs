using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dev.Acadmy.Colleges;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Subjects
{
    public  class Subject : AuditedAggregateRoot<Guid>  
    {
        public string Name { get; set; }
        public Guid? CollegeId { get; set; }
        [ForeignKey(nameof(CollegeId))]
        public College? College { get; set; }
        public ICollection<Courses.Course> Courses { get; set; } = new List<Courses.Course>();
    }
}
