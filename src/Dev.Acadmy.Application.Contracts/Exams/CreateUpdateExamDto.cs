using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Exams
{
    public class CreateUpdateExamDto
    {
        public string Name { get; set; }
        public int TimeExam { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Guid> QuestionIds { get; set; } = new List<Guid>();
    }
}
