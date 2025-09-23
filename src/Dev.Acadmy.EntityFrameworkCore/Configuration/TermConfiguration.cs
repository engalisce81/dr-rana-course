using Dev.Acadmy.Universites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class TermConfiguration : IEntityTypeConfiguration<Term>
    {
        public void Configure(EntityTypeBuilder<Term> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Terms" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasMany(x => x.Subjects).WithOne(x => x.Term).HasForeignKey(x => x.TermId);
        }   

    
    }
}
