using Dev.Acadmy.Lectures;
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
    public class LectureStudentConfiguration:IEntityTypeConfiguration<LectureStudent>
    {
        public void Configure(EntityTypeBuilder<LectureStudent> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "LectureStudents" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.Lecture).WithMany().HasForeignKey(x => x.LectureId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

        }
    }
}
