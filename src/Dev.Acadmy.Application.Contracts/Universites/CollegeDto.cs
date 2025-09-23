using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Universites
{
    public class CollegeDto :EntityDto<Guid>
    {
        public string Name { get; set; }
        public Guid? UniversityId { get; set; }
        public string? UniversityName { get; set; }

    }
}
