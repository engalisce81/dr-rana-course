using Dev.Acadmy.Universites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class GradeLevelConfiguration : IEntityTypeConfiguration<GradeLevel>
    {
        public void Configure(EntityTypeBuilder<GradeLevel> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "GradeLevels" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasOne(x => x.College).WithMany(x => x.GradeLevels).HasForeignKey(x => x.CollegeId);

        }   
    
    }
}
