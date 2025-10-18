using Dev.Acadmy.Quizzes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class QuizStudentAnswerConfiguration : IEntityTypeConfiguration<QuizStudentAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizStudentAnswer> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "QuizStudentAnswers" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.QuizStudent).WithMany(x => x.Answers).HasForeignKey(x => x.QuizStudentId);
            builder.HasOne(x => x.Question).WithMany().HasForeignKey(x => x.QuestionId);
        }
    
    }
}
