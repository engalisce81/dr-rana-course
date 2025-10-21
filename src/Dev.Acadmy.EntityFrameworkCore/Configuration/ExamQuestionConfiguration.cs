using Dev.Acadmy.Universites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev.Acadmy.Exams;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class ExamQuestionConfiguration : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "ExamQuestions" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.Question).WithMany().HasForeignKey(x => x.QuestionId);
            builder.HasOne(x => x.Exam).WithMany().HasForeignKey(x => x.ExamId);
        }
    }
}
