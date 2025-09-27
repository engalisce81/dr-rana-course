using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Lectures
{
    public class CreateUpdateLectureDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public string PdfUrl { get; set; }
        public Guid ChapterId { get; set; }
        public bool IsVisible { get; set; }
        public int QuizTime { get; set; }
    }
}
