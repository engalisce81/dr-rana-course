﻿using Dev.Acadmy.Courses;
using System;
using System.Collections.Generic;

using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Colleges
{
    public class College : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public ICollection <Courses.Course> Courses { get; set; }   = new List<Courses.Course>();
    }
}
