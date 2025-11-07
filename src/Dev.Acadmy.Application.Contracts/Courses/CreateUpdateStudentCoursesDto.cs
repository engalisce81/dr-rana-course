using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Courses
{
    public class CreateUpdateStudentCoursesDto
    {
        public Guid UserId { get; set; }
        public List<Guid> CourseIds { get; set; }
    }
}
