﻿
using Dev.Acadmy;
using Dev.Acadmy.Courses;
using Dev.Acadmy.Questions;
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
    public class CourseConfiguration : IEntityTypeConfiguration<Courses.Course>
    {
        public void Configure(EntityTypeBuilder<Courses.Course> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix +"Courses" + AcadmyConsts.DbSchema);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.Description)
                   .HasMaxLength(2000);

            builder.HasOne(x => x.College)
                   .WithMany()
                   .HasForeignKey(x => x.CollegeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.QuestionBanks)
                   .WithOne(x=>x.Course)
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    
    }

    public class CourseStudentsConfiguration : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "CourseStudentes" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();

            builder.HasIndex(x => new { x.UserId, x.CourseId }).IsUnique();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.IsSubscibe).HasDefaultValue(false);
        }
    }
}
