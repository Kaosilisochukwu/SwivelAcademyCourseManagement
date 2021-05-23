using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyStudentManagement.Test.Helpers
{
    internal static class StudentHelpers
    {
        internal static IEnumerable<Student> GetStudents()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = "1",
                    FirstName = "Steve",
                    LastName = "John",
                    Email = "Steve@j.com",
                    PhoneNumber = "08065697896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new Student
                { 
                    Id = "2",
                    FirstName = "Paul",
                    LastName = "Johse",
                    Email = "paul@j.com",
                    PhoneNumber = "08068957896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },     
                new Student
                { 
                    Id = "3",
                    FirstName = "Julia",
                    LastName = "Paul",
                    Email = "julia@p.com",
                    PhoneNumber = "08067417896",
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },      
                new Student
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
        internal static Student GetStudent()
        {
            return new Student
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
