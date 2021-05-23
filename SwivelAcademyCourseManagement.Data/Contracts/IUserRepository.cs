using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Contracts
{
    public interface IUserRepository<T> where T : class, new()
    {
        Task AddCourse(int courseId, string teacherId);
        Task AddManyCourses(IEnumerable<int> courseIds, string teacherId);
        Task<IEnumerable<Course>> GetCourses(string userId);
        Task RemoveCourse(int courseId, string teacherId);
        Task RemoveCourses(IEnumerable<int> courseIds, string teacherId);
    }
}
