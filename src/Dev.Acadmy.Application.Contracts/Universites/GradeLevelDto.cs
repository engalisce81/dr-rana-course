using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Universites
{
    public class GradeLevelDto: EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid CollegeId { get; set; }
        public string CollegeName { get; set; }
    }
}
