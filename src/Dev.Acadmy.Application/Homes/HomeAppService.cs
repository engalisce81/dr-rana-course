using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dev.Acadmy.Homes
{
    public class HomeAppService :ApplicationService
    {
        private readonly HomeManager _homeManager;
        public HomeAppService(HomeManager homeManager)
        {
            _homeManager = homeManager;
        }
        [Authorize]
        public async Task<HomesDto> GetHomeStatisticsAsync() => await _homeManager.GetHomeStatisticsAsync();
        
    }
}
