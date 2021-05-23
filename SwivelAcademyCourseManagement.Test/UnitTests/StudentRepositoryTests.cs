using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Data;
using SwivelAcademyCourseManagement.Data.Repository;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Test.Helpers;
using SwivelAcademyStudentManagement.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.UnitTests
{
    public class StudentRepositoryTests
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
            var studentRepository = new StudentRepository(context);
            var course = StudentHelpers.GetStudent();

            //Act
            await studentRepository.Insert(course);
            var returnedStudents = await studentRepository.GetAll();

            //Assert
            Assert.Single(returnedStudents);
            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.Insert(null));
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
            var studentRepository = new StudentRepository(context);
            var students = StudentHelpers.GetStudents();

            //Act
            context.AddRange(students);
            context.SaveChanges();
            var returnedCourses = await studentRepository.GetAll();

            //Assert
            Assert.Equal(students.Count(), returnedCourses.Count());
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
            var studentRepository = new StudentRepository(context);
            var student = StudentHelpers.GetStudent();
            //Act
            await studentRepository.Insert(student);
            var returnedCourse = await studentRepository.Get(x => x.Id == "5");

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
            var studentRepository = new StudentRepository(context);
            var student = StudentHelpers.GetStudent();

            //Act
            await studentRepository.Insert(student);

            await studentRepository.Delete(student);
            var returnedCourses = await studentRepository.GetAll();

            //Assert
            Assert.Empty(returnedCourses);
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => studentRepository.Delete(null));
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
            var studentRepository = new StudentRepository(context);
            var student = StudentHelpers.GetStudent();

            //Act
            await studentRepository.Insert(student);
            student.FirstName = "Frank";
            student.LastName = "James";
            
            await studentRepository.Update(student);
            var returnedStudent = await studentRepository.Get(x => x.Id == "5");

            //Assert
            Assert.Equal("Frank", returnedStudent.FirstName);
            Assert.Equal("James", returnedStudent.LastName);
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
            var studentRepository = new StudentRepository(context);
            var courseRepository = new CourseRepository(context);

            var student = StudentHelpers.GetStudent();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await studentRepository.Insert(student);
            
            await studentRepository.AddCourse(1, "5");
            await studentRepository.AddCourse(2, "5");
            var numberOfCoursesAfterInsertion = (await studentRepository.GetCourses("5")).Count();
            await studentRepository.RemoveCourse(1, "5");
            var numberOfCoursesAfterRemoval = (await studentRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(2, numberOfCoursesAfterInsertion);
            Assert.Equal(1, numberOfCoursesAfterRemoval);
            await studentRepository.RemoveCourse(2, "5");
            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.GetCourses("5"));
            
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
            var studentRepository = new StudentRepository(context);
            var courseRepository = new CourseRepository(context);

            var student = StudentHelpers.GetStudent();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await studentRepository.Insert(student);

            await studentRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5");
            var numberOfCoursesAfterInsertion = (await studentRepository.GetCourses("5")).Count();
            await studentRepository.RemoveCourses(new List<int> { 1, 2 }, "5");
            var numberOfCoursesAfterRemoval = (await studentRepository.GetCourses("5")).Count();

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
            var studentRepository = new StudentRepository(context);
            var courseRepository = new CourseRepository(context);

            var student = StudentHelpers.GetStudent();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await studentRepository.Insert(student);

            await studentRepository.AddCourse(1, "5");
            await studentRepository.AddCourse(2, "5");
            await studentRepository.AddCourse(3, "5");
            var numberOfCoursesAfterInsertion = (await studentRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(3, numberOfCoursesAfterInsertion);
            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.AddCourse(1, "5"));

            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.AddCourse(9, "5"));
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
            var studentRepository = new StudentRepository(context);
            var courseRepository = new CourseRepository(context);

            var student = StudentHelpers.GetStudent();
            var courses = CourseHelpers.GetCourses();

            //Act
            foreach (var course in courses)
            {
                await courseRepository.Insert(course);
            }
            await studentRepository.Insert(student);

            await studentRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5");
            var numberOfCoursesAfterInsertion = (await studentRepository.GetCourses("5")).Count();

            //Assert
            Assert.Equal(3, numberOfCoursesAfterInsertion);
            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.AddManyCourses(new List<int> { 1, 2, 3 }, "5"));

            await Assert.ThrowsAsync<AppUserException>(
                () => studentRepository.AddManyCourses(new List<int> { 10, 29, 13 }, "5"));
        }


    }
}
