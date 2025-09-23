using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Universites
{
    public class SubjectDto :EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid? TermId { get; set; }
        public string? TermName { get; set; }
        public Guid? GradeLevelId { get; set; }
        public string? GradeLevelName { get; set; }  

    }
}
