using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.Quizzes
{
    public class QuizResultDto
    {
       public Guid QuizId { get; set; } 
       public double TotalScore { get; set; } 
       public double StudentScore { get; set; }
       public int MyTryCount { get; set; }
       public int LectureTryCount { get; set; }
       public bool IsSuccesful { get; set; }
    }
}
