namespace Dev.Acadmy;

public static class AcadmyConsts
{
    public const string DbTablePrefix = "App";

    public const string DbSchema = "Progres";
}

public static class SetPropConsts
{
    public const string CollegeId = "CollegeId";
    public const string AccountTypeId = "AccountTypeId";
    public const string Gender = "Gender";
}

public static class RoleConsts
{
    public const string Student = "Student";
    public const string Teacher = "Teacher";
    public const string Admin = "Admin";
}


public static class AccountTypeConsts
{
    public const string Student = "Student";
    public const string Teacher = "Teacher";
    public const string Admin = "Admin";
}

public static class QuestionTypeConsts
{
    public const string MCQ = "MCQ";
    public const string TrueOrFalse = "TrueOrFalse";
    public const string ShortAnswer = "ShortAnswer";
    public const string CompleteAnswer = "CompleteAnswer";
}

public enum AccountTypeKey
{
    Admin = 1,
    Teacher = 2,
    Student = 3,
}

public enum QuestionTypeEnum
{
    MCQ = 1,
    TrueOrFalse = 2,
    ShortAnswer = 3,
    CompleteAnswer = 4,
}
