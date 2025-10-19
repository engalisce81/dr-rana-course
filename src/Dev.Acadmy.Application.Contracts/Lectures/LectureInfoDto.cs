﻿using Dev.Acadmy.Quizzes;
using System;
using System.Collections.Generic;
namespace Dev.Acadmy.Lectures
{
    public class LectureInfoDto
    {
        public Guid LectureId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public ICollection<string> PdfUrls { get; set; }  = new List<string>();  
        public QuizInfoDto Quiz { get; set; }
    }
}
