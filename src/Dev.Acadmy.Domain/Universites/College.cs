using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Universites
{
    public class College : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid? UniversityId { get; set;}
        [ForeignKey(nameof(UniversityId))]  
        public University? University { get; set; }
        public ICollection<GradeLevel> GradeLevels { get; set; } = new List<GradeLevel>();
        public ICollection <Courses.Course> Courses { get; set; }   = new List<Courses.Course>();
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
