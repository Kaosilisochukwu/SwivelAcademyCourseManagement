using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Repository
{
    public class TeacherRepository : BaseRepository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext _context;
        public TeacherRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddCourse(int courseId, string teacherId)
        {
            var teacher = await Get(x => x.Id == teacherId);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (teacher is null)
                throw new AppUserException("student does not exist");
            if (course is null)
                throw new AppUserException($"Course with id {courseId} does not exist");

            if (teacher.Courses is not null && teacher.Courses.Count + 1 > 3)
                throw new AppUserException("Cannot take more than three courses");
            if (teacher.Courses.Select(s => s.Id).Contains(courseId))
                throw new AppUserException("Student has already registered for the course");

            teacher.Courses.Add(course);
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }
        public async override Task<Teacher> Get(Expression<Func<Teacher, bool>> query) =>
                   await _context.Teachers.Include(student => student.Courses).FirstOrDefaultAsync(query);
        public async override Task<IEnumerable<Teacher>> GetAll()
        {
            return await _context.Teachers.Include(x => x.Courses).ToListAsync();
        }
        public async override Task Insert(Teacher entity)
        {
            if (entity == null)
                throw new AppUserException("Teacher object Must not be null");

            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddManyCourses(IEnumerable<int> courseIds, string teacherId)
        {
            var teacher = await Get(x => x.Id == teacherId);
            var courses = _context.Courses.Where(x => courseIds.Contains(x.Id));

            if (teacher is null)
                throw new AppUserException("student does not exist");
            if (courses.Count() != courseIds.Count())
                throw new AppUserException("One or more courses does not exist");

            if (teacher.Courses is not null && teacher.Courses.Count + courses.Count() > 3)
                throw new AppUserException("Cannot take more than three courses");

            if (courseIds.Any(x => teacher.Courses.Select(s => s.Id).Contains(x)))
                throw new AppUserException("Student has already registered one or more course");

            teacher.Courses.AddRange(courses);
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetCourses(string teacherId)
        {
            var teacher = await Get(x => x.Id == teacherId);
            if (teacher is null)
                throw new AppUserException("student does not exist");

            if (teacher.Courses is null || teacher.Courses.Count < 1)
                throw new AppUserException("Student has not registered course");

            return teacher.Courses;
        }

        public async Task RemoveCourse(int courseId, string teacherId)
        {
                var teacher = await Get(x => x.Id == teacherId);
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
                if (teacher is null)
                    throw new AppUserException("Teacher does not exist");

                if (course is null)
                    throw new AppUserException("Course does not exist");

                if (teacher.Courses is null || teacher.Courses.Count < 1)
                    throw new AppUserException("Teacher has no registered course");

                if (!teacher.Courses.Select(s => s.Id).Contains(courseId))
                    throw new AppUserException("Teacher does not take course with selected Id");

                teacher.Courses.Remove(course);
                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();
        }

        public async Task RemoveCourses(IEnumerable<int> courseIds, string teacherId)
        {
            var teacher = await Get(x => x.Id == teacherId);
            courseIds = courseIds.Distinct();
            if (teacher is null)
                throw new AppUserException("Teacner does not exist");

            var courses = _context.Courses.Where(x => courseIds.Contains(x.Id));

            if (courseIds.Count() != courses.Count())
                throw new AppUserException("One or more courses does not exist");

            if (teacher.Courses is null || teacher.Courses.Count < 1)
                throw new AppUserException("Teacher has no registered course");

            if (courseIds.Any(x => !teacher.Courses.Select(s => s.Id).Contains(x)))
                throw new AppUserException("Teacher does not take one or more courses with selected Id");

            teacher.Courses.RemoveAll(x => courses.Contains(x));
            _context.Teachers.Update(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
