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
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly IMapper _mapper;

        public TeacherController(ITeacherRepository teacherRepo, IMapper mapper)
        {
            _teacherRepo = teacherRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers a new Teacher
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
        /// <returns>Created teacher object</returns>
        /// <response code="201">If teacher is registered successfully</response>
        /// <response code="400">if there are validation Errors</response> 
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("registerTeacher", Name = "registerTeacher")]
        public async Task<IActionResult> Registerteacher(TeacherToRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var teacher = _mapper.Map<Teacher>(model);
                    teacher.Id = Guid.NewGuid().ToString();
                    teacher.CreatedOn = DateTime.Now;
                    teacher.ModifiedOn = DateTime.Now;
                    await _teacherRepo.Insert(teacher);
                    return Created("registerteacher", new ResponseModel("00", "Success", teacher));
                }
                catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
            }
            else
                return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
        }

        /// <summary>
        /// updates a Teacher detail
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /Register
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
        public async Task<IActionResult> Updateteacher([FromBody] TeacherToUpdateDTO model)
        {
            var teacher = await _teacherRepo.Get(x => x.Id == model.Id);
            if (teacher is not null)
            {
                teacher = _mapper.Map<TeacherToUpdateDTO, Teacher>(model, teacher);
                if (ModelState.IsValid)
                {
                    try
                    {
                        teacher.ModifiedOn = DateTime.Now;
                        var updatedteacher = await _teacherRepo.Update(teacher);
                        return Ok(new ResponseModel("00", "Success", teacher));
                    }
                    catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
                }
                else

                    return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
            }
            return BadRequest(new ResponseModel("22", "teacher does not exist", null));

        }


        /// <summary>
        /// Deletes a user with a specified Id
        /// </summary>
        /// <param name="id">Teacher id</param>
        /// <returns>null</returns>
        /// <response code="200">If teacher deleted successfully</response>
        /// <response code="400">If the item is null</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteteacher(string id)
        {
            try
            {
                var teacher = await _teacherRepo.Get(x => x.Id == id);
                await _teacherRepo.Delete(teacher);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Adds a course to a Teacher
        /// </summary>
        /// <param name="teacherId">Teacher Id</param>
        /// <param name="courseId">Course id</param>
        /// <returns>Null</returns>
        /// <response code="200">If course updates successfully</response>
        /// <response code="400">If the item is not successfully added to the student</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{teacherId}/addCourse")]
        public async Task<IActionResult> AddCourse(string teacherId, int courseId)
        {
            try
            {
                await _teacherRepo.AddCourse(courseId, teacherId);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

        /// <summary>
        /// Adds a List of courses to a user's registered teacher
        /// </summary>
        /// <param name="id">Teacher id</param>
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
                await _teacherRepo.AddManyCourses(courseIds, id);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }


        /// <summary>
        /// Removes a course from the list of a Teacher's registered courses
        /// </summary>
        /// <param name="id">Teacher Id</param>
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
                var teacher = await _teacherRepo.Get(x => x.Id == id);
                if (teacher is not null)
                {
                    await _teacherRepo.RemoveCourse(courseId, id);
                    return Ok(new ResponseModel("00", "Success", null));
                }
                return BadRequest(new ResponseModel("22", "teacher does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }


        /// <summary>
        /// Removes one or more courses form teacher registered courses
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
                await _teacherRepo.RemoveCourses(courseIds, id);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }


        /// <summary>
        /// Fetches all registered Teacher
        /// </summary>
        /// <returns>All Registered Teachers</returns>
        /// <response code="200">If teacher are fetched sccessfully</response>
        /// <response code="400">If teachers are not successfully fetched</response> 
        /// <response code="404">If entity is null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> Getteachers()
        {
            try
            {

                var teachers = await _teacherRepo.GetAll();
                if (teachers is not null && teachers.Any()) return Ok(new ResponseModel("00", "Success", teachers.Select(teacher => new
                {
                    teacher.Id,
                    teacher.FirstName,
                    teacher.LastName,
                    teacher.Email,
                    teacher.PhoneNumber,
                    Courses = teacher.Courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })
                })));
                return BadRequest(new ResponseModel("22", "There are no teachers in the database", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }

        }


        /// <summary>
        /// Gets the teacher with a specified Id
        /// </summary>
        /// <param name="id">teacher Id</param>
        /// <returns>A Student with the specified id</returns>
        /// <response code="200">If teacher is successfully fetched</response>
        /// <response code="400">If the teacher is not successfully fetched</response> 
        /// <response code="404">If student does not exist</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Getteacher(string id)
        {
            try
            {
                var teacher = await _teacherRepo.Get(x => x.Id == id);
                if (teacher is not null) return Ok(new ResponseModel("00", "Success", new
                {
                    teacher.Id,
                    teacher.FirstName,
                    teacher.LastName,
                    teacher.Email,
                    teacher.PhoneNumber,
                    Courses = teacher.Courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })
                }));
                return BadRequest(new ResponseModel("22", "teacher does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }


        /// <summary>
        /// Gets all the courses registered by the Teacher
        /// </summary>
        /// <param name="id">teacher Id</param>
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
                var teacher = await _teacherRepo.Get(x => x.Id == id);
                if (teacher is not null)
                {
                    var courses = teacher.Courses;
                    return Ok(new ResponseModel("00", "Success", courses.Select(s => new
                    {
                        s.Id,
                        s.Title,
                        s.CourseDescription,
                        s.Level,
                        s.Price
                    })));
                }
                return BadRequest(new ResponseModel("22", "teacher does not exist", null));
            }
            catch (Exception ex) { return BadRequest(new ResponseModel("22", "Failed", new { ex.Message })); }
        }

    }

}
