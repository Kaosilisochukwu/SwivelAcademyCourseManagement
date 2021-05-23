using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Test.Helpers
{
    internal static class TeacherHelpers
    {
        internal static IEnumerable<Teacher> GetTeachers()
        {
            return new List<Teacher>
            {
                new Teacher
                {
                    Id = "1",
                    FirstName = "Steve",
                    LastName = "John",
                    Email = "Steve@j.com",
                    PhoneNumber = "08065697896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new Teacher
                {
                    Id = "2",
                    FirstName = "Paul",
                    LastName = "Johse",
                    Email = "paul@j.com",
                    PhoneNumber = "08068957896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new Teacher
                {
                    Id = "3",
                    FirstName = "Julia",
                    LastName = "Paul",
                    Email = "julia@p.com",
                    PhoneNumber = "08067417896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new Teacher
                {
                    Id = "4",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Email = "jane@d.com",
                    PhoneNumber = "08025697896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                }
            };
        }
        internal static Teacher GetTeacher()
        {
            return new Teacher
            {
                Id = "5",
                FirstName = "Prince",
                LastName = "Peter",
                Email = "peter@j.com",
                PhoneNumber = "08035797896",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };
        }
    }
}
