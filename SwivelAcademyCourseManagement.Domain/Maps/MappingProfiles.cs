using AutoMapper;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Models;

namespace SwivelAcademyCourseManagement.Domain.Maps
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TeacherToRegisterDTO, Teacher>();
            CreateMap<StudentToRegisterDTO, Teacher>();
            CreateMap<StudentToUpdateDTO, Student>();
            CreateMap<TeacherToUpdateDTO, Teacher>();
            CreateMap<CourseToAddDTO, Course>();
            CreateMap<CourseToUpdateDTO, Course>();
        }
    }
}
