using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Supports
{
    public class SupportDto:AuditedEntityDto<Guid>
    {
        public string FaceBook { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
