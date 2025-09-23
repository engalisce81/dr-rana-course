using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Universites
{
    public class CreateUpdateCollegeDto
    {
        public string Name { get; set; }
        public Guid? UniversityId { get; set; }
        public int GradeLevelCount { get; set; }
    }
}
