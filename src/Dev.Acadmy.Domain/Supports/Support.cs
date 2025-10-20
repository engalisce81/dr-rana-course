using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Dev.Acadmy.Supports
{
    public class Support:AuditedAggregateRoot<Guid> 
    {
        public string FaceBook { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }   
    }
}
