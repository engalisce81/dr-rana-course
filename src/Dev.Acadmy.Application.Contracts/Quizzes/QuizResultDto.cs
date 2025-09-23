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
    }
}
