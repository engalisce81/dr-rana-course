﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Subjects
{
    public class SubjectDto :EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid? CollegeId { get; set; }

    }
}
