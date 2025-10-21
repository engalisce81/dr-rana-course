using Dev.Acadmy.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Exams" + AcadmyConsts.DbSchema);
            builder.ConfigureByConvention();
            builder .HasOne(x=>x.Course).WithMany(x=>x.Exams).HasForeignKey(x=>x.CourseId);
        }
    }
}
