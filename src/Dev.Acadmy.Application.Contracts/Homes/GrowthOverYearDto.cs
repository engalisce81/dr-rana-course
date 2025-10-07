using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Homes
{
    public class GrowthOverYearDto
    {
        public List<MonthlyCountDto> Students { get; set; } = new();
        public List<MonthlyCountDto> Teachers { get; set; } = new();
    }

}
