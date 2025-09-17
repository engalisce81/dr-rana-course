using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Chapters
{
    public class CreateUpdateChapterDto
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
    }
}
