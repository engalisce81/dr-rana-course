using Dev.Acadmy.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class ExamQuestionBankConfiguration :IEntityTypeConfiguration<ExamQuestionBank>
    {
        public void Configure(EntityTypeBuilder<ExamQuestionBank> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "ExamQuestionBanks" + AcadmyConsts.DbSchema);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.Exam).WithMany().HasForeignKey(x => x.ExamId);
            builder.HasOne(x => x.QuestionBank).WithMany().HasForeignKey(x => x.QuestionBankId);
        }
    }
}
