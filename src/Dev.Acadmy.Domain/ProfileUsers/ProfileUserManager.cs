﻿using Dev.Acadmy.AccountTypes;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Response;
using Dev.Acadmy.Universites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.ProfileUsers
{
    public class ProfileUserManager :DomainService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityUserRepository _userRepository;
        private readonly MediaItemManager _mediaItemManager;
        private readonly IRepository<AccountType, Guid> _accountTypeRepository;
        private readonly IRepository<College, Guid> _collegeRepository;
        public ProfileUserManager(IRepository<College, Guid> collegeRepository, IRepository<AccountType, Guid> accountTypeRepository, MediaItemManager mediaItemManager, ICurrentUser currentUser, IIdentityUserRepository userRepository) 
        {
            _collegeRepository = collegeRepository;
            _accountTypeRepository = accountTypeRepository;
            _mediaItemManager = mediaItemManager;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<ResponseApi<UserInfoDto>> GetUserInfoAsync()
        {
            var userInfo = await GetUserDataAsync();
            return new ResponseApi<UserInfoDto> { Data = userInfo, Success = true, Message = "get user info success" };
        }

        public async Task<ResponseApi<UserInfoDto>> UpdateAllUserDataAsync(UpdateProfielDto input)
        {
            var userInfo = await GetUserDataAsync();
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            if(input.CollegeId != null)
            {
                userInfo.CollegeId = input.CollegeId;
                currentUser.SetProperty(SetPropConsts.CollegeId,input.CollegeId);
            }
            if(input.Name != null)
            {
                userInfo.Name = input.Name;
                currentUser.Name=input.Name;
            }
            if (input.LogoUrl != null) 
            {
                userInfo.ProfilePictureUrl= input.LogoUrl;
                await _mediaItemManager.UpdateAsync(currentUser.Id ,new CreateUpdateMediaItemDto { RefId=currentUser.Id,Url=input.LogoUrl}); 
            }
            await _userRepository.UpdateAsync(currentUser);
            return new ResponseApi<UserInfoDto> { Data = userInfo, Success = true, Message = "update all profiel data success" };
        }

        private async Task<UserInfoDto> GetUserDataAsync()
        {
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var media = await _mediaItemManager.GetAsync(currentUser.Id);
            var collegeId = currentUser.GetProperty<Guid>(SetPropConsts.CollegeId);
            var accountTypeId = currentUser.GetProperty<Guid>(SetPropConsts.AccountTypeId);
            var college = await _collegeRepository.GetAsync(collegeId);
            var accountType = await _accountTypeRepository.GetAsync(accountTypeId);
            var userInfo = new UserInfoDto
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                Name = currentUser.Name,
                ProfilePictureUrl = media?.Url ?? string.Empty,
                CollegeId = college?.Id ?? null,
                CollegeName = college?.Name ?? string.Empty,
                AccountTypeId = accountType?.Id ?? null,
                AccountTypeKey = accountType?.Key.ToString() ?? string.Empty,
                AccountTypeName = accountType?.Name ?? string.Empty
            };
            return userInfo;
        }



    }
}
