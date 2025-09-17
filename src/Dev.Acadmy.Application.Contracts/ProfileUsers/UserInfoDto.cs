using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Acadmy.ProfileUsers
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        // College Info
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }

        // Account Type Info
        public Guid? AccountTypeId { get; set; }
        public string AccountTypeKey { get; set; }
        public string AccountTypeName { get; set; }
    }
}
