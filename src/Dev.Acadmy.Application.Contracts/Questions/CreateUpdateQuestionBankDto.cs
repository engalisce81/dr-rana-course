﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Questions
{
    public class CreateUpdateQuestionBankDto
    {
        public string Name { get; set; }
        public Guid CreatorId { get; set; }
        public Guid CourseId { get; set; }
    }
}
