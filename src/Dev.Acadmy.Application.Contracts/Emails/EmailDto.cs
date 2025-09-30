using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Emails
{
    public class EmailDto: EntityDto<Guid>
    {
        public string EmailAdrees { get; set; }
        public string Code { get; set; }
        public bool IsAccept { get; set; }
       
    }
}
