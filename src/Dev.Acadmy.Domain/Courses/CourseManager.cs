﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Dev.Acadmy.MediaItems;
using Dev.Acadmy.Questions;
using Dev.Acadmy.Response;
using Dev.Acadmy.LookUp;

namespace Dev.Acadmy.Courses
{
    public class CourseManager : DomainService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly MediaItemManager _mediaItemManager; 
        private readonly IIdentityUserRepository _userRepository;
        private readonly QuestionBankManager _questionBankManager;
        private readonly IRepository<CourseStudent, Guid> _courseStudentRepository;
        public CourseManager(IRepository<CourseStudent, Guid> courseStudentRepository, QuestionBankManager questionBankManager, IIdentityUserRepository userRepository, MediaItemManager mediaItemManager, ICurrentUser currentUser, IRepository<Course> courseRepository , IMapper mapper) 
        {
            _courseStudentRepository = courseStudentRepository;
            _questionBankManager = questionBankManager;
            _userRepository = userRepository;
            _mediaItemManager = mediaItemManager;
            _currentUser = currentUser;
            _mapper = mapper;
            _courseRepository = courseRepository;
        }

        public async Task<ResponseApi<CourseDto>> GetAsync(Guid id)
        {
            var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return new ResponseApi<CourseDto> { Data = null, Success = false, Message = "Not found Course" };
            var dto = _mapper.Map<CourseDto>(course);
            var mediaItem = await _mediaItemManager.GetAsync(dto.Id);
            dto.LogoUrl = mediaItem?.Url ?? "";
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "find succeess" };
        }

        public async Task<PagedResultDto<CourseDto>> GetListAsync(int pageNumber, int pageSize, string? search)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var queryable = await _courseRepository.GetQueryableAsync();
            if (!string.IsNullOrWhiteSpace(search)) queryable = queryable.Include(x=>x.College).Where(c => c.Name.Contains(search));
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var courses = new List<Course>();
            if (roles.Any(x=>x.Name.ToUpper() ==RoleConsts.Admin.ToUpper() )) courses = await AsyncExecuter.ToListAsync(queryable.Include(x => x.College).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            else courses = await AsyncExecuter.ToListAsync(queryable.Where(c => c.UserId == _currentUser.GetId()).Include(x => x.College).OrderByDescending(c => c.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize));
            var courseDtos = _mapper.Map<List<CourseDto>>(courses);
            foreach (var courseDto in courseDtos)
            {
                var mediaItem = await _mediaItemManager.GetAsync(courseDto.Id);
                courseDto.LogoUrl = mediaItem?.Url??"";
            }
            return new PagedResultDto<CourseDto>(totalCount, courseDtos);
        }

        public async Task<ResponseApi<CourseDto>> CreateAsync(CreateUpdateCourseDto input)
        {
            var course = _mapper.Map<Course>(input);
            course.UserId=_currentUser.GetId(); 
            var result = await _courseRepository.InsertAsync(course);
            await _mediaItemManager.CreateAsync(new CreateUpdateMediaItemDto { Url =input.LogoUrl, RefId = result.Id});
            await _questionBankManager.CreateAsync(new CreateUpdateQuestionBankDto {CreatorId =result.UserId, CourseId = result.Id, Name = $"{result.Name} Question Bank" });
            var dto = _mapper.Map<CourseDto>(result);
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "save succeess" };
        }

        public async Task<ResponseApi<CourseDto>> UpdateAsync(Guid id, CreateUpdateCourseDto input)
        {
            var courseDB = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (courseDB == null) return new ResponseApi<CourseDto> { Data = null, Success = false, Message = "Not found Course" };
            var course = _mapper.Map(input, courseDB);
            var result = await _courseRepository.UpdateAsync(course);
            await _mediaItemManager.UpdateAsync(id, new CreateUpdateMediaItemDto { Url = input.LogoUrl, RefId = result.Id });
            var questionBank = await _questionBankManager.GetByCourse(id);
            if(questionBank !=null) await _questionBankManager.UpdateAsync(questionBank.Id, new CreateUpdateQuestionBankDto { CreatorId=result.UserId,CourseId = result.Id, Name = $"{result.Name} Question Bank" });
            var dto = _mapper.Map<CourseDto>(result);
            return new ResponseApi<CourseDto> { Data = dto, Success = true, Message = "update succeess" };
        }
        public async Task<ResponseApi<bool>> DeleteAsync(Guid id)
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());
            var course = await _courseRepository.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null) return new ResponseApi<bool> { Data = false, Success = false, Message = "Not found Course" };
            if(roles.Any(x=>x.Name.ToUpper() != RoleConsts.Admin.ToUpper()) && course.UserId != _currentUser.GetId()) return new ResponseApi<bool> { Data = false, Success = false, Message = "you not allowed to delete this course" };
            await _courseRepository.DeleteAsync(course);
            await _mediaItemManager.DeleteAsync(id);
            var questionBank = await _questionBankManager.GetByCourse(id);
            if(questionBank!=null) await _questionBankManager.DeleteAsync(questionBank.Id);
            return new ResponseApi<bool> { Data = true, Success = true, Message = "delete succeess" };
        }

        public async Task<PagedResultDto<LookupDto>> GetCoursesListAsync()
        {
            var roles = await _userRepository.GetRolesAsync(_currentUser.GetId());

            var queryable = await _courseRepository.GetQueryableAsync();
            var totalCount = await AsyncExecuter.CountAsync(queryable);
            var courses = new List<Course>();
            if(roles.Any(x=>x.Name.ToUpper()==RoleConsts.Admin)) courses = await AsyncExecuter.ToListAsync(queryable.OrderByDescending(c => c.CreationTime));
            else courses = await AsyncExecuter.ToListAsync(queryable.Where(c => c.UserId == _currentUser.GetId()).OrderByDescending(c => c.CreationTime));
            var courseDtos = _mapper.Map<List<LookupDto>>(courses);
            return new PagedResultDto<LookupDto>(totalCount, courseDtos);
        }

        public async Task<PagedResultDto<CourseInfoDto>> GetCoursesInfoListAsync(int pageNumber,int pageSize,string? search)
        {
            var queryable = await _courseRepository.GetQueryableAsync();
            var currentUser = await _userRepository.GetAsync( _currentUser.GetId());
            var collegeId = currentUser.GetProperty<Guid?>(SetPropConsts.CollegeId);
            if (collegeId == null || collegeId == Guid.Empty) return new PagedResultDto<CourseInfoDto>(0, new List<CourseInfoDto>());
            var studentCourses = await(await _courseStudentRepository.GetQueryableAsync()).Where(x => x.UserId == currentUser.Id && x.IsSubscibe).Select(x => x.CourseId).ToListAsync();
            if (!string.IsNullOrWhiteSpace(search))queryable = queryable.Where(c =>c.Name.Contains(search) || c.Description.Contains(search));
            queryable = queryable.Where(c => c.CollegeId == collegeId.Value);
            var totalCount = await queryable.CountAsync();
            var courses = await queryable.Include(c => c.User).Include(c => c.College).OrderByDescending(c => c.CreationTime).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var courseDtos = new List<CourseInfoDto>();
            foreach (var course in courses)
            {
                var media = await _mediaItemManager.GetAsync(course.Id);                
                courseDtos.Add(new CourseInfoDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Description = course.Description,
                    Price = course.Price,
                    LogoUrl = media?.Url ??"",
                    UserId = course.UserId,
                    UserName = course.User?.Name??"",
                    CollegeId = course.CollegeId,
                    CollegeName = course.College?.Name ?? "",
                    AlreadyJoin = studentCourses.Contains(course.Id)
                });
            }
            return new PagedResultDto<CourseInfoDto>(totalCount, courseDtos);
        }
    }
}
