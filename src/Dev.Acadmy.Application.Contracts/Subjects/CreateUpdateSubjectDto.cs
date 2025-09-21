using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Subjects
{
    public class CreateUpdateSubjectDto
    {
        public string Name { get; set; }
        public Guid? CollegeId { get; set; }
    }
}
