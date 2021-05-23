using SwivelAcademyCourseManagement.Domain.Entities;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;

namespace SwivelAcademyCourseManagement.Test.Helpers
{
    internal static class CourseHelpers
    {
        internal static IEnumerable<Course> GetCourses()
        {
            return new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Title = "Getting Started with JavaScript.",
                    CourseDescription = "Beginner guide to Javascript.",
                    Level = Level.Begginner,
                    Price = 500,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                      
                new Course
                {
                    Id = 2,
                    Title = "Advanced JavaScript.",
                    CourseDescription = "Advanced guide to JS",
                    Level = Level.Advanced,
                    Price = 600,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },     
                new Course
                {
                    Id = 3,
                    Title = "Intermediate JavaScript.",
                    CourseDescription = "A next to Beginner guide to Javascriptt.",
                    Level = Level.Begginner,
                    Price = 1000,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },      
                new Course
                {
                    Id = 4,
                    Title = "Generics In C#.",
                    CourseDescription = "Complete guide to generics in C#.",
                    Level = Level.Intermediate,
                    Price = 700,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                }
            };
        }
        internal static IEnumerable<Course> GetThreeCourses()
        {
            return new List<Course>
            {                      
                new Course
                {
                    Id = 7,
                    Title = "Advanced JavaScript.",
                    CourseDescription = "Advanced guide to JS",
                    Level = Level.Advanced,
                    Price = 600,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },      
                new Course
                {
                    Id = 8,
                    Title = "Intermediate JavaScript.",
                    CourseDescription = "A next to Beginner guide to Javascriptt.",
                    Level = Level.Begginner,
                    Price = 1000,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },      
                new Course
                {
                    Id = 9,
                    Title = "Generics In C#.",
                    CourseDescription = "Complete guide to generics in C#.",
                    Level = Level.Intermediate,
                    Price = 700,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                }
            };
        }
        internal static Course GetCourse()
        {
            return new Course
            {
                Id = 5,
                Title = "Getting Started with JavaScript.",
                CourseDescription = "Beginner guide to Javascript.",
                Level = Level.Begginner,
                Price = 500,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }
    }
}
