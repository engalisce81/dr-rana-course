using Dev.Acadmy.Courses;
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
    public class CourseInfoConfiguration : IEntityTypeConfiguration<CourseInfo>
    {
        public void Configure(EntityTypeBuilder<CourseInfo> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "CourseInfos" + AcadmyConsts.DbSchema);
            builder.ConfigureByConvention();

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.HasOne(x => x.Course).WithMany(x => x.CourseInfos).HasForeignKey(x => x.CourseId);

        }
    }
}
