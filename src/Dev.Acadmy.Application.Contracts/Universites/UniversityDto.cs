using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dev.Acadmy.Universites
{
    public class UniversityDto :AuditedEntityDto<Guid>
    {
        public string Name { get; set; }

    }
}
