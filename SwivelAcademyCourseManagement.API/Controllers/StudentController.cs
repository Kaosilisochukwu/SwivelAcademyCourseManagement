using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IMapper _mapper;

        public StudentController(IStudentRepository studentRepo, IMapper mapper)
        {
            _studentRepo = studentRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new student
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        FirstName = "Paul",
        ///        LastName = "Johse",
        ///        Email = "paul@j.com",
        ///        PhoneNumber = "08068957896",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">object details to be added</param>
        /// <returns>Created student object</returns>
        /// <response code="201">If Student is registered successfully</response>
        /// <response code="400">if there are validation Errors</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("registerStudent", Name = "registerStudent")]
        public async Task<IActionResult> RegisterStudent(StudentToRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Student = _mapper.Map<Student>(model);
                    Student.Id = Guid.NewGuid().ToString();
                    Student.CreatedOn = DateTime.Now;
                    Student.ModifiedOn = DateTime.Now;
                    await _studentRepo.Insert(Student);
                    return Created("registerStudent", new ResponseModel("00", "Success", Student));
                }
                catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
            }
            else
                return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
        }

        /// <summary>
        /// updates a Student detail
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        Id = "guidId",
        ///        FirstName = "Paul",
        ///        LastName = "Johse",
        ///        Email = "paul@j.com",
        ///        PhoneNumber = "08068957896",
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Details of object to update</param>
        /// <returns>Updted details</returns>
        /// <response code="200">If student updates successfully</response>
        /// <response code="400"> if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentToUpdateDTO model)
        {
            var student = await _studentRepo.Get(x => x.Id == model.Id);
            if (student is not null)
            {
                student = _mapper.Map<StudentToUpdateDTO, Student>(model, student);
                if (ModelState.IsValid)
                {
                    try
                    {
                        student.ModifiedOn = DateTime.Now;
                        var updatedStudent = await _studentRepo.Update(student);
                        return Ok(new ResponseModel("00", "Success", student));
                    }
                    catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
                }
                else

                    return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
            }
            return BadRequest(new ResponseModel("22", "Student does not exist", null));

        }

        /// <summary>
        /// Deletes a user with a specified Id
        /// </summary>
        /// <param name="id">Student id</param>
        /// <returns>null</returns>
        /// <response code="200">If Student deleted successfully</response>
        /// <response code="400">If the item is null</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                var Student = await _studentRepo.Get(x => x.Id == id);
                await _studentRepo.Delete(Student);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Adds a course to a Student
        /// </summary>
        /// <param name="studentId">Student Id</param>
        /// <param name="courseId">Course id</param>
        /// <returns>Null</returns>
        /// <response code="200">If course updates successfully</response>
        /// <response code="400">If the item is not successfully added to the student</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{studentId}/addCourse")]
        public async Task<IActionResult> AddCourse(string studentId, int courseId)
        {
            try
            {
                await _studentRepo.AddCourse(courseId, studentId);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }



        /// <summary>
        /// Adds a List of courses to a user's registered student
        /// </summary>
        /// <param name="id">Student id</param>
        /// <param name="courseIds">Course Id</param>
        /// <returns>Null</returns>
        /// <response code="200">If course is added successfully</response>
        /// <response code="400">If the item not successfully added</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}/addCourses")]
        public async Task<IActionResult> AddCourses(IEnumerable<int> courseIds, string id)
        {
            try
            {
                await _studentRepo.AddManyCourses(courseIds, id);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Removes a course from the list of a user's registered courses
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <param name="courseId">Course Id</param>
        /// <returns>Null</returns>
        /// <response code="200">If course removed successfully</response>
        /// <response code="400">If the item is not successfully removed</response> 
        /// <response code="404">If student does not exist is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("{id}/removeCourse/{courseId}")]
        public async Task<IActionResult> RemoveCourse(string id, int courseId)
        {
            try
            {
                var Student = await _studentRepo.Get(x => x.Id == id);
                if (Student is not null)
                {
                    await _studentRepo.RemoveCourse(courseId, id);
                    return Ok(new ResponseModel("00", "Success", null));
                }
                return NotFound(new ResponseModel("22", "Student does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Removes one or more courses form student registered courses
        /// </summary>
        /// <param name="courseIds">A collection of course ids</param>
        /// <param name="id">Student Id</param>
        /// <returns>Null</returns>
        /// <response code="200">If course removed successfully</response>
        /// <response code="400">If the item is not successfully renoved</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id}/removeCourses")]
        public async Task<IActionResult> RemoveCourses(IEnumerable<int> courseIds, string id)
        {
            try
            {
                await _studentRepo.RemoveCourses(courseIds, id);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Fetches all registered Student
        /// </summary>
        /// <returns>All Registered Students</returns>
        /// <response code="200">If student are fetched sccessfully</response>
        /// <response code="400">If students are not successfully fetched</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {

                var Students = await _studentRepo.GetAll();
                if (Students is not null && Students.Any()) return Ok(new ResponseModel("00", "Success", Students.Select(student => new
                {
                    student.Id,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.PhoneNumber,
                    Courses = student.Courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })
                })));
                return BadRequest(new ResponseModel("22", "There are no Students in the database", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }

        }

        /// <summary>
        /// Gets the student with a specified Id
        /// </summary>
        /// <param name="id">Student Id</param>
        /// <returns>A Student with the specified id</returns>
        /// <response code="200">If student is successfully fetched</response>
        /// <response code="400">If the student is not successfully fetched</response> 
        /// <response code="404">If student does not exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            try
            {
                var student = await _studentRepo.Get(x => x.Id == id);
                if (student is not null) return Ok(new ResponseModel("00", "Success", new
                {
                    student.Id,
                    student.FirstName,
                    student.LastName,
                    student.Email,
                    student.PhoneNumber,
                    Courses = student.Courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })
                }));
                return NotFound(new ResponseModel("22", "Student does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Gets all the courses registered by the Student
        /// </summary>
        /// <param name="id">studednt Id</param>
        /// <returns></returns>
        /// <response code="200">If courses are fetched successfully</response>
        /// <response code="400">If the courses are not successfully fetched</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/getCourses")]
        public async Task<IActionResult> GetCourses(string id)
        {
            try
            {
                var student = await _studentRepo.Get(x => x.Id == id);
                if (student is not null) 
                {
                    var courses = student.Courses;
                    return Ok(new ResponseModel("00", "Success", courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })));
                }
                return BadRequest(new ResponseModel("22", "Student does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }
    }
}
