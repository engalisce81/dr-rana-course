﻿
using Dev.Acadmy.Chapters;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Universites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace Dev.Acadmy.Courses
{
    public class Course : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Title { get; set;}
        public string Description { get; set;}
        public decimal Price { get; set;}
        public float Rating { get; set; } // average rating
        public Guid UserId { get; set; }
        public Guid? CollegeId { get; set;}
        public Guid? SubjectId { get; set; }

        // Visibility
        public bool IsActive { get; set; } = true; // enabled/disabled

        // Purchase & duration
        public bool IsLifetime { get; set; } = false;
        public int? DurationInDays { get; set; } // null if lifetime
        [ForeignKey(nameof(CollegeId))]
        public College College { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public QuestionBank QuestionBank { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
        public ICollection<Chapter>  Chapters { get; set; } = new List<Chapter>();
        public ICollection<CourseInfo> CourseInfos { get; set; } = new List<CourseInfo>();
    }
}
