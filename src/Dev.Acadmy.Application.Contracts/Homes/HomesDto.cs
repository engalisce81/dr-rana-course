using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Homes
{
    public class HomesDto
    {
        public string NameFiledOne { get; set; }
        public double CountFiledOne { get; set; }
        public double PercentageFiledOne { get; set; }
        public string NameFiledTwo { get; set; }
        public double CountFiledTwo { get; set; }
        public double PercentageFiledTwo { get; set; }
        public string NameFiledThree { get; set; }
        public double CountFiledThree { get; set; }
        public double PercentageFiledThree { get; set; }
        public List<MemberDto> Members { get; set; } = new List<MemberDto>();
        public GrowthOverYearDto GrowthOverYear { get; set; }


    }
}
