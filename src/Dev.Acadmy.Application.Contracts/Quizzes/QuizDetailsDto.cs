using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
namespace Dev.Acadmy.Quizzes
{
    public class QuizDetailsDto
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public int QuizTime { get; set; }
        public List<QuestionDetailesDto> Questions { get; set; }
    }
}
