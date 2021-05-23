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
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {

        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddCourse(int courseId, string studentId)
        {
            var student = await Get(x => x.Id == studentId);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (student is null)
                throw new AppUserException("student does not exist");
            if (course is null)
                throw new AppUserException($"Course with id {courseId} does not exist");

            if (student.Courses is not null && student.Courses.Count + 1 > 3)
                throw new AppUserException("Cannot take more than three courses");
            if (student.Courses.Select(s => s.Id).Contains(courseId))
                throw new AppUserException("Student has already registered for the course");

            student.Courses.Add( course );
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
        public async override Task<Student> Get(Expression<Func<Student, bool>> query) =>
                   await _context.Students.Include(student => student.Courses).FirstOrDefaultAsync(query);
        public async override Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.Include(x => x.Courses).ToListAsync();
        }
        public async Task AddManyCourses(IEnumerable<int> courseIds, string studentId)
        {
            var student = await Get(x => x.Id == studentId);
            courseIds = courseIds.Distinct();
            var courses = _context.Courses.Where(x => courseIds.Contains(x.Id));

            if (student is null)
                throw new AppUserException("student does not exist");
            if (courses.Count() != courseIds.Count())
                throw new AppUserException("One or more courses does not exist");

            if (student.Courses is not null && student.Courses.Count + courses.Count() > 3)
                throw new AppUserException("Cannot take more than three courses");

            if (courseIds.Any(x => student.Courses.Select(s => s.Id).Contains(x)))
                throw new AppUserException("Student has already registered one or more course");

            student.Courses.AddRange(courses);
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
        public async override Task Insert(Student entity)
        {
            if (entity == null)
                throw new AppUserException("Student object Must not be null");

            _context.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetCourses(string studentId)
        {
            var student = await Get(x => x.Id == studentId);
            if (student is null)
                throw new AppUserException("student does not exist");

            if (student.Courses is null || student.Courses.Count < 1)
                throw new AppUserException("Student has not registered course");

            return student.Courses;
        }

        public async Task RemoveCourse(int courseId, string studentId)
        {
            var student = await Get(x => x.Id == studentId);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (student is null)
                throw new AppUserException("student does not exist");

            if (course is null)
                throw new AppUserException("Course does not exist");

            if (student.Courses is null || student.Courses.Count < 1)
                throw new AppUserException("Student has no registered course");

            if (!student.Courses.Select(s => s.Id).Contains(courseId))
                throw new AppUserException("Student does not offer course with selected Id");

            student.Courses.Remove(course);
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCourses(IEnumerable<int> courseIds, string studentId)
        {
            var student = await Get(x => x.Id == studentId);
            courseIds = courseIds.Distinct();
            if (student is null)
                throw new AppUserException("Student does not exist");

            var courses = _context.Courses.Where(x => courseIds.Contains(x.Id));

            if (courseIds.Count() != courses.Count())
                throw new AppUserException("One or more courses does not exist");

            if (student.Courses is null || student.Courses.Count < 1)
                throw new AppUserException("Student has no registered course");

            if (courseIds.Any(x => !student.Courses.Select(s => s.Id).Contains(x)))
                throw new AppUserException("Student does not offer one or more courses with selected Id");

            student.Courses.RemoveAll(x => courses.Contains(x));
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
