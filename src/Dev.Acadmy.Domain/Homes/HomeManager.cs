using Dev.Acadmy.Courses;
using Dev.Acadmy.Universites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Dev.Acadmy.Homes
{
    public class HomeManager : DomainService
    {
        private readonly IRepository<Courses.Course, Guid> _courseRepository;
        private readonly IRepository<CourseStudent, Guid> _courseStudentRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IdentityUserManager _userManager;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<University,Guid> _universityRepository;
        private readonly IRepository<College, Guid> _collegeRepository;
        private readonly IRepository<GradeLevel, Guid> _gradeLevelRepository;
        public HomeManager(IRepository<College, Guid> collegeRepository, IRepository<GradeLevel, Guid> gradeLevelRepository, IRepository<University, Guid> universityRepository, ICurrentUser currentUser, IdentityUserManager userManager, IIdentityUserRepository userRepository, IRepository<Courses.Course, Guid> courseRepository , IRepository<CourseStudent, Guid> courseStudentRepository)
        {
            _collegeRepository = collegeRepository;
            _gradeLevelRepository = gradeLevelRepository;
            _universityRepository = universityRepository;
            _currentUser = currentUser;
            _courseRepository = courseRepository;
            _courseStudentRepository = courseStudentRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<HomesDto> GetHomeStatisticsAsync()
        {
            var currentUserId = _currentUser.GetId() ;
            var currentUser = await _userManager.GetByIdAsync(currentUserId);
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            if (userRoles.Any(x=>x.ToLower() ==RoleConsts.Admin.ToLower()))  return await GetAdminHomeStatisticsAsync();
            if (userRoles.Any(x=>x.ToLower() == RoleConsts.Teacher.ToLower())) return await GetTeacherHomeStatisticsAsync(currentUserId);
            throw new UserFriendlyException("Only Admins or Teachers can access this dashboard.");
        }

        private async Task<HomesDto> GetAdminHomeStatisticsAsync()
        {
            var now = DateTime.UtcNow;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);
            var lastMonthEnd = currentMonthStart.AddDays(-1);

            // 🔹 الطلاب
            var userStudents = await _userManager.GetUsersInRoleAsync(RoleConsts.Student);
            var studentsLastMonthTotal = userStudents.Count(u => u.CreationTime < currentMonthStart && u.CreationTime >= lastMonthStart);
            var studentsCurrentMonthTotal = userStudents.Count(u => u.CreationTime >= currentMonthStart);
            var totalStudentsNow = userStudents.Count;
            var studentIncreasePercentage = CalculatePercentageIncrease(totalStudentsNow, studentsLastMonthTotal);

            // 🔹 المعلمين
            var userTeachers = await _userManager.GetUsersInRoleAsync(RoleConsts.Teacher);
            var teachersLastMonthTotal = userTeachers.Count(u => u.CreationTime < currentMonthStart && u.CreationTime >= lastMonthStart);
            var teachersCurrentMonthTotal = userTeachers.Count(u => u.CreationTime >= currentMonthStart);
            var totalTeachersNow = userTeachers.Count;
            var teacherIncreasePercentage = CalculatePercentageIncrease(totalTeachersNow, teachersLastMonthTotal);

            // 🔹 الكورسات
            var allCourses = await _courseRepository.GetListAsync();
            var coursesLastMonthTotal = allCourses.Count(c => c.CreationTime < currentMonthStart && c.CreationTime >= lastMonthStart);
            var coursesCurrentMonthTotal = allCourses.Count(c => c.CreationTime >= currentMonthStart);
            var totalCoursesNow = allCourses.Count;
            var courseIncreasePercentage = CalculatePercentageIncrease(totalCoursesNow, coursesLastMonthTotal);

            // 🔹 الجامعات أو الأعضاء
            var members = await GetUniversityMembersCountAsync();

            var growthOver =  await  GetStudentTeacherGrowthOverYearAsync();

            return new HomesDto
            {
                NameFiledOne = "Number of Students",
                CountFiledOne = totalStudentsNow,
                PercentageFiledOne = studentIncreasePercentage,

                NameFiledTwo = "Number of Teachers",
                CountFiledTwo = totalTeachersNow,
                PercentageFiledTwo = teacherIncreasePercentage,

                NameFiledThree = "Number of Courses",
                CountFiledThree = totalCoursesNow,
                PercentageFiledThree = courseIncreasePercentage,

                Members = members,

                GrowthOverYear =growthOver
            };
        }
        private async Task<HomesDto> GetTeacherHomeStatisticsAsync(Guid teacherId)
        {
            var teacherCourses = await ( await _courseRepository.GetQueryableAsync()).Where(x=>x.UserId == teacherId).ToListAsync();
            var teacherCourseIds = teacherCourses.Select(x => x.Id).ToList();

            var allCourseStudents = await (await _courseStudentRepository.GetQueryableAsync()).Where(x => teacherCourseIds.Contains(x.CourseId)).ToListAsync();
            var totalStudents = allCourseStudents.Count;

            var needSubscribeCount = allCourseStudents.Count(x => x.IsSubscibe == false);

            var totalCourses = teacherCourses.Count;

            var studentPercentage = totalStudents > 0 ? 100 : 0;
            var needSubPercentage = totalStudents == 0 ? 0 : Math.Round((double)needSubscribeCount / totalStudents * 100, 2);
            var coursePercentage = 100;
            var members = await GetCollegeGradeLevelMembersAsync();
            return new HomesDto
            {
                NameFiledOne = "Number of Students",
                CountFiledOne = totalStudents,
                PercentageFiledOne = studentPercentage,

                NameFiledTwo = "Number of NeedSubscribe",
                CountFiledTwo = needSubscribeCount,
                PercentageFiledTwo = needSubPercentage,

                NameFiledThree = "Number of Courses",
                CountFiledThree = totalCourses,
                PercentageFiledThree = coursePercentage,

                Members = members,
            };
        }

        private double CalculatePercentageIncrease(int currentTotal, int previousPeriodTotal)
        {
            if (previousPeriodTotal == 0)
                return currentTotal > 0 ? 100 : 0;

            var increase = currentTotal - previousPeriodTotal;
            return Math.Round(((double)increase / previousPeriodTotal) * 100, 2);
        }
        public async Task<List<MemberDto>> GetUniversityMembersCountAsync()
        {
            var universities = await _universityRepository.GetListAsync();
            var allUsers = await _userRepository.GetListAsync();
            var result = new List<MemberDto>();
            foreach (var university in universities)
            {
                int count = 0;
                foreach (var user in allUsers)
                {
                    var userUniversityId = user.GetProperty<Guid?>(SetPropConsts.UniversityId);
                    if (userUniversityId.HasValue && userUniversityId == university.Id)
                    {
                        count++;
                    }
                }
                result.Add(new MemberDto
                {
                    Id = university.Id,
                    MemberName = university.Name,
                    MembersCount = count
                });
            }
            return result.OrderByDescending(x => x.MembersCount) // ترتيب تنازلي حسب عدد الأعضاء
                .ToList();
        }

        public async Task<List<MemberDto>> GetCollegeGradeLevelMembersAsync()
        {
            // 🔹 احصل على المستخدم الحالي (المدرس)
            var currentUser = await _userRepository.GetAsync(_currentUser.GetId());
            var collegeId = currentUser.GetProperty<Guid>(SetPropConsts.CollegeId);

            if (collegeId == Guid.Empty)
                throw new UserFriendlyException("المدرس الحالي لا ينتمي إلى أي كلية.");

            // 🔹 احصل على المراحل الدراسية التابعة للكلية الحالية
            var gradeLevelsQuery = await _gradeLevelRepository.GetQueryableAsync();
            var gradeLevels = await gradeLevelsQuery
                .Where(x => x.CollegeId == collegeId)
                .ToListAsync();

            if (gradeLevels == null || !gradeLevels.Any())
                throw new UserFriendlyException("لا توجد مراحل دراسية في هذه الكلية.");

            // 🔹 احصل على الطلاب فقط
            var userStudents = await _userManager.GetUsersInRoleAsync(RoleConsts.Student);

            // 🔹 احسب عدد الطلاب في كل مرحلة داخل الكلية الحالية
            var result = gradeLevels
                .Select(gl => new MemberDto
                {
                    Id = gl.Id,
                    MemberName = gl.Name,
                    MembersCount = userStudents.Count(s =>
                        s.GetProperty<Guid>(SetPropConsts.CollegeId) == collegeId &&
                        s.GetProperty<Guid>(SetPropConsts.GradeLevelId) == gl.Id)
                })
                .Where(x => x.MembersCount > 0) // فقط المراحل اللي فيها طلاب
                .ToList();

            return result;
        }


        private async Task<GrowthOverYearDto> GetStudentTeacherGrowthOverYearAsync()
        {
            var currentYear = DateTime.UtcNow.Year;

            // جلب المستخدمين حسب الدور
            var students = await _userManager.GetUsersInRoleAsync(RoleConsts.Student);
            var teachers = await _userManager.GetUsersInRoleAsync(RoleConsts.Teacher);

            // نجهز قائمة الأشهر من 1 إلى 12
            var months = Enumerable.Range(1, 12).ToList();

            var result = new GrowthOverYearDto();

            foreach (var month in months)
            {
                var monthStart = new DateTime(currentYear, month, 1);
                var nextMonthStart = month == 12
                    ? new DateTime(currentYear + 1, 1, 1)
                    : new DateTime(currentYear, month + 1, 1);

                // عدد الطلاب المسجلين في هذا الشهر
                var studentCount = students.Count(u =>
                    u.CreationTime >= monthStart && u.CreationTime < nextMonthStart);

                // عدد المعلمين المسجلين في هذا الشهر
                var teacherCount = teachers.Count(u =>
                    u.CreationTime >= monthStart && u.CreationTime < nextMonthStart);

                // نضيفهم في النتيجة
                result.Students.Add(new MonthlyCountDto
                {
                    Month = monthStart.ToString("MMMM"),
                    Count = studentCount
                });

                result.Teachers.Add(new MonthlyCountDto
                {
                    Month = monthStart.ToString("MMMM"),
                    Count = teacherCount
                });
            }

            return result;
        }


    }
}
