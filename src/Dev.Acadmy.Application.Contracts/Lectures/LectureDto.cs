using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Lectures
{
    public class LectureDto:EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public Guid ChapterId { get; set; }
        public string ChapterName { get; set; }
        public Guid QuizId { get; set; }
        public string QuizName { get; set; }
        public int QuizTime { get; set; }
        public bool IsVisible { get; set; }
    }
}
