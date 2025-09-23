using Dev.Acadmy.Lectures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Chapters
{
    public class CourseChaptersDto
    {
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }
        public List<LectureInfoDto> Lectures { get; set; } = new();
    }
}
