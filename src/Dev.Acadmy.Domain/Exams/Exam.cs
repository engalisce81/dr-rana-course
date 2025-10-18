﻿using Dev.Acadmy.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Exams
{
    public class Exam:AuditedAggregateRoot<Guid>
    {
        public string Name {  get; set; }
        public int TimeExam { get; set; }
        public int Score { get; set; }
        public bool IsActive {  get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Courses.Course Course { get; set; }  
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
