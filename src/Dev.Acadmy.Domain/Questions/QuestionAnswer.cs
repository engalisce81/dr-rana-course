using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Questions
{
    public class QuestionAnswer :AuditedAggregateRoot<Guid>
    {
        public string Answer {  get; set; }
        public bool IsCorrect { get; set; }
        public Guid QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
    }
}
