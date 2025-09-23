using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Universites
{
    public class GradeLevelDto
    {
        public string Name { get; set; }
        public Guid CollegeId { get; set; }
        public string CollegeName { get; set; }
    }
}
