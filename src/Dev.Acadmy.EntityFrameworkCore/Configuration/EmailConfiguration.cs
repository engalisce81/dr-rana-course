using Dev.Acadmy.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class EmailConfiguration :IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Emails" + AcadmyConsts.DbSchema);
            builder.ConfigureByConvention();
        }
    } 
}
