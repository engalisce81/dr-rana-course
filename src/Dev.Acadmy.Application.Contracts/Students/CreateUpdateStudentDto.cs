using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Students
{
    public class CreateUpdateStudentDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public Guid CollegeId { get; set; }
        public Guid UniversityId { get; set; }
        public Guid? GradeLevelId { get; set; }
        public int AccountTypeKey { get; set; }
        public string? StudentMobileIP { get; set; }
    }
}
