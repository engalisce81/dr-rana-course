using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Volo.Abp.EntityFrameworkCore.Modeling;
using Dev.Acadmy;
using Dev.Acadmy.Colleges;

namespace Dev.Acadmy.Configuration
{
    public class CollegeConfiguration : IEntityTypeConfiguration<College>
    {
        public void Configure(EntityTypeBuilder<College> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Colleges" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(x => x.Courses)
               .WithOne(c => c.College)
               .HasForeignKey(c => c.CollegeId)
               .OnDelete(DeleteBehavior.Restrict); // مهم
        }
    }
}
