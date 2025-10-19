using Dev.Acadmy.Lectures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Chapters
{
    public class Chapter:AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public bool IsFree { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Courses.Course Course { get; set; }
        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
    }
}
