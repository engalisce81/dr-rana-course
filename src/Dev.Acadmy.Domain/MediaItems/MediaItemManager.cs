﻿using AutoMapper;
using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.MediaItems
{
    public class MediaItemManager:DomainService
    {
        private readonly IRepository<MediaItem, Guid> _mediaItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MediaItemManager(IRepository<MediaItem, Guid> mediaItemRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mediaItemRepository = mediaItemRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<MediaItem?> GetAsync(Guid refId) => await _mediaItemRepository.FirstOrDefaultAsync(x => x.RefId == refId);


        public async Task<MediaItem> CreateAsync(CreateUpdateMediaItemDto input)
        {
            var mediaItem = _mapper.Map<MediaItem>(input);
            var result = await _mediaItemRepository.InsertAsync(mediaItem);
            return result;
        }

        public async Task<MediaItem> UpdateAsync(Guid id, CreateUpdateMediaItemDto input)
        {
            var mediaItemDB = await _mediaItemRepository.FirstOrDefaultAsync(x => x.RefId == id);
            var result = new MediaItem();
            if (mediaItemDB == null) result = await CreateAsync(input);
            else
            {

                DeleteImageByUrlAsync(mediaItemDB.Url);
                var mediaItem = _mapper.Map(input, mediaItemDB);
                result = await _mediaItemRepository.UpdateAsync(mediaItem);
            }
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var mediaItem = await _mediaItemRepository.FirstOrDefaultAsync(x => x.RefId == id);
            if (mediaItem != null)
            {
                DeleteImageByUrlAsync(mediaItem.Url);
                await _mediaItemRepository.DeleteAsync(mediaItem);
            }
        }

        private void DeleteImageByUrlAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return; // مفيش URL
                }

                // لازم يكون URL يبدأ بدومين المشروع
                if (!url.StartsWith("https://scola-dev-be.demo.egisg.com"))
                {
                    return; // مش صورة عندنا
                }

                var relativePath = new Uri(url).AbsolutePath;
                if (relativePath.StartsWith("/"))
                    relativePath = relativePath.Substring(1);

                var wwwRootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");
                var filePath = Path.Combine(wwwRootPath, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                // هنا تسجل اللوج بس، ما توقفش التعديل كله
                Console.WriteLine($"DeleteImageByUrl Error: {ex.Message}");
            }
        }

        public async Task<ResponseApi<string>> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return new ResponseApi<string> { Data = null, Message = "file not found", Success = false };
            // تأكد من وجود فولدر images
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);
            // اسم الملف
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(imagesPath, fileName);
            // حفظ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // جلب BaseUrl
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            // URL كامل للملف
            var fileUrl = $"{baseUrl}/images/{fileName}";
            return new ResponseApi<string> { Data = fileUrl, Message = "save success", Success = true };
        }
    }
}

