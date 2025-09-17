﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class CreateUpdateQuestionDto
    {
        public string Title { get; set; }
        public Guid QuestionTypeId { get; set; }
        public Guid QuizId { get; set; }
        public Guid QuestionBankId { get; set; }
        public int Score { get; set; }
    }
}
