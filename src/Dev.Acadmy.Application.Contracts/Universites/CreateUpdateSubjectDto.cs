using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Universites
{
    public class CreateUpdateSubjectDto
    {
        public string Name { get; set; }
        public Guid? TermId { get; set; }
        public Guid? GradeLevelId { get; set; }
    }
}
