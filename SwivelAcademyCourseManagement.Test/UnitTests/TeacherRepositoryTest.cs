using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Data;
using SwivelAcademyCourseManagement.Data.Repository;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.UnitTests
{
    public class TeacherRepositoryTest
    {
        [Fact]
        public async void Insert_SavesDataAndThrowsForNullData()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var course = TeacherHelpers.GetTeacher();

            //Act
            await TeacherRepository.Insert(course);
            var returnedteachers = await TeacherRepository.GetAll();

            //Assert
            Assert.Single(returnedteachers);
            await Assert.ThrowsAsync<AppUserException>(
                () => TeacherRepository.Insert(null));
        }


        [Fact]
        public async void GetAll_GetsAllSavedCourses()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var teachers = TeacherHelpers.GetTeachers();

            //Act
            context.AddRange(teachers);
            context.SaveChanges();
            var returnedCourses = await TeacherRepository.GetAll();

            //Assert
            Assert.Equal(teachers.Count(), returnedCourses.Count());
        }

        [Fact]

        public async void Get_ReturnsACourseWithASpecificId_ThrowIfCourseDoesNotExist()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var teacherRepository = new TeacherRepository(context);
            var teacher = TeacherHelpers.GetTeacher();

            //Act
            await teacherRepository.Insert(teacher);
            var returnedCourse = await teacherRepository.Get(x => x.Id == "5");

            //Assert
            Assert.Equal("5", returnedCourse.Id);
        }

        [Fact]
        public async void Delete_RemovesACourseFromDatabase_ThrowsForNonNullObject()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var teacherRepository = new TeacherRepository(context);
            var teacher = TeacherHelpers.GetTeacher();

            //Act
            await teacherRepository.Insert(teacher);
            await teacherRepository.Delete(teacher);
            var returnedCourses = await teacherRepository.GetAll();

            //Assert
            Assert.Empty(returnedCourses);
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => teacherRepository.Delete(null));
        }
        [Fact]
        public async void Update_ModifiesASavedData()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var teacher = TeacherHelpers.GetTeacher();

            //Act
            await TeacherRepository.Insert(teacher);

            teacher.FirstName = "Frank";
            teacher.LastName = "James";

            await TeacherRepository.Update(teacher);
            var returnedteacher = await TeacherRepository.Get(x => x.Id == "5");

            //Assert
            Assert.Equal("Frank", returnedteacher.FirstName);
            Assert.Equal("James", returnedteacher.LastName);
        }

        [Fact]
        public async void RemoveCourse_RemovesAcourseWithSameIdFromTheListOfSudentCourses()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var courseRepository = new CourseRepository(context);

            var teacher = TeacherHelpers.GetTeacher();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await TeacherRepository.Insert(teacher);

            await TeacherRepository.AddCourse(1, "5");
            await TeacherRepository.AddCourse(2, "5");
            var numberOfCoursesAfterInsertion = (await TeacherRepository.GetCourses("5")).Count();
            await TeacherRepository.RemoveCourse(1, "5");
            var numberOfCoursesAfterRemoval = (await TeacherRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(2, numberOfCoursesAfterInsertion);
            Assert.Equal(1, numberOfCoursesAfterRemoval);
            await TeacherRepository.RemoveCourse(2, "5");
            await Assert.ThrowsAsync<AppUserException>(
                () => TeacherRepository.GetCourses("5"));

        }


        [Fact]
        public async void RemoveCourses_RemovesCoursesWithSameIdFromTheListOfSudentCourses()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var courseRepository = new CourseRepository(context);

            var teacher = TeacherHelpers.GetTeacher();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await TeacherRepository.Insert(teacher);

            await TeacherRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5");
            var numberOfCoursesAfterInsertion = (await TeacherRepository.GetCourses("5")).Count();
            await TeacherRepository.RemoveCourses(new List<int> { 1, 2 }, "5");
            var numberOfCoursesAfterRemoval = (await TeacherRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(3, numberOfCoursesAfterInsertion);
            Assert.Equal(1, numberOfCoursesAfterRemoval);
        }

        [Fact]
        public async void AddCourse_AddsAcourseWithSameIdFromTheListOfSudentCourses_ThrowsIfUserTriesToRegisterMoreThan3Courses()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var TeacherRepository = new TeacherRepository(context);
            var courseRepository = new CourseRepository(context);

            var teacher = TeacherHelpers.GetTeacher();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await TeacherRepository.Insert(teacher);

            await TeacherRepository.AddCourse(1, "5");
            await TeacherRepository.AddCourse(2, "5");
            await TeacherRepository.AddCourse(3, "5");
            var numberOfCoursesAfterInsertion = (await TeacherRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(3, numberOfCoursesAfterInsertion);
            await Assert.ThrowsAsync<AppUserException>(
                () => TeacherRepository.AddCourse(1, "5"));

            await Assert.ThrowsAsync<AppUserException>(
                () => TeacherRepository.AddCourse(9, "5"));
        }

        [Fact]
        public async void AddCourses_AddsCoursesWithSameIdFromTheListOfSudentCourses()
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var teacherRepository = new TeacherRepository(context);
            var courseRepository = new CourseRepository(context);

            var teacher = TeacherHelpers.GetTeacher();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await teacherRepository.Insert(teacher);

            await teacherRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5");
            var numberOfCoursesAfterInsertion = (await teacherRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(3, numberOfCoursesAfterInsertion);
            await Assert.ThrowsAsync<AppUserException>(
                () => teacherRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5"));

            await Assert.ThrowsAsync<AppUserException>(
                () => teacherRepository.AddManyCourses(new List<int> { 10, 29, 13 }, "5"));
        }
    }
}
