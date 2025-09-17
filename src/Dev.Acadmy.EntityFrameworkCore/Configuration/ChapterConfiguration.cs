using Dev.Acadmy;
using Dev.Acadmy.Chapters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Chapters" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.Course).WithMany(x => x.Chapters).HasForeignKey(x => x.CourseId);
        }
    }
}
