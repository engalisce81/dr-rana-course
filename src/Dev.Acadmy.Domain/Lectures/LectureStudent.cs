using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Dev.Acadmy.Lectures
{
    public class LectureStudent : AuditedAggregateRoot<Guid>
    {
        public Guid LectureId { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(LectureId))]
        public Lecture Lecture { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public int AttemptsUsed { get; set; }
        public int MaxAttempts { get; set; } // ممكن نخزنها في Lecture نفسه
        public bool IsCompleted =>  AttemptsUsed>= MaxAttempts;

    }
}
