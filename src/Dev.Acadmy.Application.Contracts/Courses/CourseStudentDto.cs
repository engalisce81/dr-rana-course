using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Courses
{
    public class CourseStudentDto:EntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreationTime { get; set; }
        public string LogoUrl { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsSubscibe { get; set; }
    }
}
