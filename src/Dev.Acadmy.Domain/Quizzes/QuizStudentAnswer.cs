using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Quizzes
{
    public class QuizStudentAnswer : AuditedAggregateRoot<Guid>
    {
        public Guid QuizStudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid? SelectedAnswerId { get; set; }
        public string? TextAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public double ScoreObtained { get; set; }

        [ForeignKey(nameof(QuizStudentId))]
        public QuizStudent QuizStudent { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public Question Question { get; set; }
    }
}
