using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Dev.Acadmy.Quizzes
{
    public class QuizStudent : AuditedAggregateRoot<Guid>
    {
        public Guid? LectureId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public int Score { get; set; }
        public int TryCount { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }

        public ICollection<QuizStudentAnswer> Answers { get; set; } = new List<QuizStudentAnswer>();
    }

}
