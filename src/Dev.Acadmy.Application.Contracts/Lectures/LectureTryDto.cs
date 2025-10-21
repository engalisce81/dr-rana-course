using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Lectures
{
    public class LectureTryDto
    {
        public int MyTryCount { get; set; }
        public int LectureTryCount { get; set; }
        public int SuccessQuizRate { get; set; }
        public int MyScoreRate { get; set; }
        public bool IsSucces { get; set; }
    }
}
