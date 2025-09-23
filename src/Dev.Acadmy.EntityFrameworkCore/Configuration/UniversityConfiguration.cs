using Dev.Acadmy.Universites;
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
    public class UniversityConfiguration : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "Universites" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();
            builder.HasMany(x=>x.Colleges).WithOne(x=>x.University).HasForeignKey(x => x.UniversityId);
        }   
    }
}
