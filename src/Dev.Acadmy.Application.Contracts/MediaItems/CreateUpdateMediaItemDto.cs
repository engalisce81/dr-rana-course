using System;
namespace Dev.Acadmy.MediaItems
{
    public class CreateUpdateMediaItemDto
    {
        public bool IsImage { get; set; }
        public string Url { get; set; }
        public Guid RefId { get; set; }
    }
}
