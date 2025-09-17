using Dev.Acadmy.Response;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.ProfileUsers
{
    public class ProfileUserAppService :ApplicationService
    {
        private readonly ProfileUserManager _profileUserManager;
        public ProfileUserAppService(ProfileUserManager profileUserManager) 
        {
            _profileUserManager = profileUserManager;
        }
        [Authorize]
        public async Task<ResponseApi<UserInfoDto>> GetUserInfoAsync() => await _profileUserManager.GetUserInfoAsync();
        [Authorize]
        public async Task<ResponseApi<UserInfoDto>> UpdateAllUserDataAsync(UpdateProfielDto input) => await _profileUserManager.UpdateAllUserDataAsync(input);

    }
}
