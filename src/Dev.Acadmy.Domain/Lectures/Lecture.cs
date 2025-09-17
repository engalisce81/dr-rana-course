
using Dev.Acadmy.Quizzes;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Lectures
{
    public class Lecture : AuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public Guid ChapterId { get; set; }
        public Guid QuizId { get; set; }
        public bool IsVisible { get; set; } 
        [ForeignKey(nameof(ChapterId))]
        public Chapters.Chapter Chapter { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set;}
    }
}
