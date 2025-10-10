using Dev.Acadmy.Emails;
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
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Exams" + AcadmyConsts.DbSchema);
            builder.ConfigureByConvention();
            builder .HasOne(x=>x.Course).WithOne(x=>x.Exam).HasForeignKey<Exam>(x=>x.CourseId);
            builder.HasMany(x => x.Questions).WithOne(x => x.Exam).HasForeignKey(x => x.ExamId);
        }
    }
}
