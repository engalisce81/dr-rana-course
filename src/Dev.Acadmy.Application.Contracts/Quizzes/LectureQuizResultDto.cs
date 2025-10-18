using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class LectureQuizResultDto
    {
        public Guid LectureId { get; set; }
        public string LectureTitle { get; set; }
        public List<QuizResultDetailDto> Quizzes { get; set; } = new();
    }

    public class QuizResultDetailDto
    {
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public double StudentScore { get; set; }
        public double TotalScore { get; set; }
        public List<QuestionResultDto> Questions { get; set; } = new();
    }

    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string? StudentAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public double ScoreObtained { get; set; }
        public double ScoreTotal { get; set; }
    }

}
