using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Emails
{
    public  class Email : AuditedAggregateRoot<Guid>
    {
        public string EmailAdrees { get; set; }
        public string Code { get; set; }
    }
}
