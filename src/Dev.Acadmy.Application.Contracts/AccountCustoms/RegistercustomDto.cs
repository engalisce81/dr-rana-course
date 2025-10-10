using System;

namespace Dev.Acadmy.AccountCustoms
{
    public class RegistercustomDto
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Gender { get; set; }
        public Guid CollegeId { get; set; }
        public Guid UniversityId { get; set; }
        public Guid? GradeLevelId { get; set; }
        public int AccountTypeKey { get; set; }
        public string? StudentMobileIP { get; set; }
    }
}
