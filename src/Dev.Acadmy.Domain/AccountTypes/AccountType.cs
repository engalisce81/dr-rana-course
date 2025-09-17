using System;

using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Services;

namespace Dev.Acadmy.AccountTypes
{
    public class AccountType :AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public int Key { get; set; }
    }
}
