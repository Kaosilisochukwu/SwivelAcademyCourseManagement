using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.Entities;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Repository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async override Task<Course> Get(Expression<Func<Course, bool>> query)
        {
            var course = await _context.Courses.Include(student => student.Teachers)
                                          .Include(x => x.Students)
                                          .FirstOrDefaultAsync(query);
            if (course is null) throw new CoursesException("Course Does not exist");
            return course;
        }
        public async override Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses.Include(x => x.Teachers)
                                        .Include(x => x.Students)
                                        .ToListAsync();
        }
        public async override Task Insert(Course entity)
        {
            if (entity == null) throw new CoursesException("Course must be provided");
            if (!Enum.IsDefined(typeof(Level), entity.Level)) throw new CoursesException("Level option is not available, Level must be within 1 to 3");
            _context.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
