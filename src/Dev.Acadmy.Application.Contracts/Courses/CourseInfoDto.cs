
using System;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Courses
{
    public class CourseInfoDto:AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
