
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.MediaItems
{
    public class MediaItem:AuditedAggregateRoot<Guid>
    {
        public bool IsImage { get;set; }
        public string Url { get; set; }
        public Guid RefId { get; set; }
    }
}
