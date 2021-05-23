using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SwivelAcademyCourseManagement.Data;
using SwivelAcademyCourseManagement.Data.Repository;
using SwivelAcademyCourseManagement.Domain.Exceptions;
using SwivelAcademyCourseManagement.Test.Helpers;
using System;
using System.Linq;
using Xunit;

namespace SwivelAcademyCourseManagement.Test.UnitTests
{
    public class CourseRepositoryTests
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
            var courseRepository = new CourseRepository(context);
            var course = CourseHelpers.GetCourse();

            //Act
            await courseRepository.Insert(course);
            var returnedCourses = await courseRepository.GetAll();

            //Assert
            Assert.Single(returnedCourses);
            await Assert.ThrowsAsync< CoursesException>(
                () =>  courseRepository.Insert(null));
        }


        [Fact]
        public async void GetAll_GetsAllSavedCourses()
        {

            // Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionBuilder.ToString());
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(connection)
                    .Options;
            using var context = new ApplicationDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            var courseRepository = new CourseRepository(context);
            var courses = CourseHelpers.GetCourses();

            //Act
            context.AddRange(courses);
            context.SaveChanges();            
            var returnedCourses = await courseRepository.GetAll();

            //Assert
            Assert.Equal(courses.Count(), returnedCourses.Count());
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
            var courseRepository = new CourseRepository(context);

            //Act
            var course = CourseHelpers.GetCourse();
            await courseRepository.Insert(course);            
            var returnedCourse = await courseRepository.Get(x => x.Id == 5);

            //Assert
            Assert.Equal(5, returnedCourse.Id);
            await Assert.ThrowsAsync<CoursesException>(
                () => courseRepository.Get(x => x.Id == 1));
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
            var courseRepository = new CourseRepository(context);
            var course = CourseHelpers.GetCourse();
            await courseRepository.Insert(course);
            
            //Act
            await courseRepository.Delete(course);

            var returnedCourses = await courseRepository.GetAll();

            //Assert
            Assert.Empty(returnedCourses); 
            await Assert.ThrowsAsync<ArgumentNullException>(
                () => courseRepository.Delete(null));
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
            var courseRepository = new CourseRepository(context);
            var course = CourseHelpers.GetCourse();


            //Act
            await courseRepository.Insert(course);

            course.Title = "Java Getting Started";
            await courseRepository.Update(course);          
            
            var returnedCourse = await courseRepository.Get(x => x.Id == 5);

            //Assert
            Assert.Equal("Java Getting Started", returnedCourse.Title);
        }
    }
}
