using System;

namespace Dev.Acadmy.Quizzes
{
    public class QuizInfoDto
    {
        public Guid QuizId { get; set; }
        public string Title { get; set; }
        public int QuestionsCount { get; set; }
        public bool AlreadyAnswer { get; set; }
    }
}
