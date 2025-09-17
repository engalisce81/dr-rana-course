using Dev.Acadmy.Lectures;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Quizzes
{
    public class Quiz : AuditedAggregateRoot<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int QuizTime { get; set; }
        public Lecture Lecture { get; set; }
    }
}
