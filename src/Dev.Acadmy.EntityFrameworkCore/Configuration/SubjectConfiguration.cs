using Dev.Acadmy.Colleges;
using Dev.Acadmy.Subjects;
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
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Subjects" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);
            builder.HasMany(x=>x.Courses).WithOne(x=>x.Subject).HasForeignKey(x=>x.SubjectId);
            builder.HasOne(x => x.College).WithMany(x => x.Subjects).HasForeignKey(x => x.CollegeId);

        }
    }
}
