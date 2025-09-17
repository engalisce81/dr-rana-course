using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Dev.Acadmy.MediaItems;

namespace Dev.Acadmy.Configuration
{
    public class MediaItemConfiguration : IEntityTypeConfiguration<MediaItem>
    {
        public void Configure(EntityTypeBuilder<MediaItem> builder)
        {
            builder.ToTable(AcadmyConsts.DbTablePrefix + "MediaItems" + AcadmyConsts.DbTablePrefix);
            builder.ConfigureByConvention();

            builder.Property(x => x.Url)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.RefId)
                   .IsRequired();
        }
    }
}
