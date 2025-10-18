using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.MediaItems
{
    public class MediaItemAppService:ApplicationService
    {
        private readonly MediaItemManager _mediaItemManager;
        public MediaItemAppService(MediaItemManager mediaItemManager)
        {
            _mediaItemManager=mediaItemManager;
        }
        [Authorize]
        public async Task<ResponseApi<string>> UploadImageAsync(IFormFile file) => await _mediaItemManager.UploadImageAsync(file);
        [Authorize]
        public async Task<PagedResultDto<string>> UploadImagesAsync(ICollection<IFormFile> files) => await  _mediaItemManager.UploadImagesAsync(files);

    }
}
