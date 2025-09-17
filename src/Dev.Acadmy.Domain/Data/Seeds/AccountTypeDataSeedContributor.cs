using Dev.Acadmy.AccountTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Dev.Acadmy.Data.Seeds
{
    public class AccountTypeDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<AccountType, Guid> _accountTypeRepository;
        public AccountTypeDataSeedContributor(IRepository<AccountType, Guid> accountTypeRepository)
        {
            _accountTypeRepository = accountTypeRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            var accountTypes = new List<AccountType>()
            {
                new AccountType{  Name=AccountTypeConsts.Admin, Key=(int)AccountTypeKey.Admin},
                new AccountType{  Name=AccountTypeConsts.Teacher, Key=(int)AccountTypeKey.Teacher},
                new AccountType{  Name=AccountTypeConsts.Student, Key=(int)AccountTypeKey.Student},
            };

            foreach (var accountType in accountTypes) if (await _accountTypeRepository.FindAsync(x => x.Key == accountType.Key) == null) await _accountTypeRepository.InsertAsync(accountType);
        }
    }
}
