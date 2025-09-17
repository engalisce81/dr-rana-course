using Dev.Acadmy.AccountTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class AccountTypeConfiguration : IEntityTypeConfiguration<AccountType>
    {
        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "AccountTypes" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
        }
    }
}
