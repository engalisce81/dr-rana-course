using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Exams
{
    public class ExamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TimeExam { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ExamQuestions> ExamQuestions { get; set; } = new List<ExamQuestions>();
    }
}
