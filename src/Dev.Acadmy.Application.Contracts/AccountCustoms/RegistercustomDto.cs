using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.AccountCustoms
{
    public class RegistercustomDto
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public Guid CollegeId { get; set; }
        public Guid AccountTypeId { get; set; }
     
    }
}
