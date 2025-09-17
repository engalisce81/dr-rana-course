using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Dev.Acadmy.Lectures;

namespace Dev.Acadmy.Configuration
{
    public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Lectures" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(x => x.Content)
                   .HasMaxLength(5000);

            builder.Property(x => x.VideoUrl)
                   .HasMaxLength(1000);

            builder.Property(x => x.IsVisible)
                   .HasDefaultValue(false);

            builder.HasOne(x => x.Chapter)
                   .WithMany(x=>x.Lectures)
                   .HasForeignKey(x => x.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}