using Dev.Acadmy.Universites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dev.Acadmy.Supports;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dev.Acadmy.Configuration
{
    public class SupportConfiguratin : IEntityTypeConfiguration<Support>
    {
        public void Configure(EntityTypeBuilder<Support> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Supports" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();


        }
    }
}
