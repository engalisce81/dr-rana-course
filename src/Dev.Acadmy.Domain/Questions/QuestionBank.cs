using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Questions
{
    public class QuestionBank :AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public Courses.Course Course { get;set; }
        public ICollection<Question> Questions { get; set; }=new List<Question>();
    }
}
