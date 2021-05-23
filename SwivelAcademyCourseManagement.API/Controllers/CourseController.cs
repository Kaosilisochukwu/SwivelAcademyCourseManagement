using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwivelAcademyCourseManagement.Data.Contracts;
using SwivelAcademyCourseManagement.Domain.DTOs;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository CourseRepo, IMapper mapper)
        {
            _courseRepo = CourseRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Registers A course
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///         Title = "Generics In C#.",
        ///         CourseDescription = "Complete guide to generics in C#.",
        ///         Level = 1,
        ///         Price = 700,
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created course</returns>
        /// <response code="201">Returns the newly created course</response>
        /// <response code="400">If the item is null or if there are validation Errors</response>  
        [HttpPost]
        [Route("register", Name = "registerCourse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegisterCourse(CourseToAddDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var Course = _mapper.Map<Course>(model);
                    Course.CreatedOn = DateTime.Now;
                    Course.ModifiedOn = DateTime.Now;
                    await _courseRepo.Insert(Course);
                    return Created("registerCourse", new ResponseModel("00", "Success", Course));
                }
                catch (Exception ex)
                {
                    return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
                }
            }
            else
                return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
        }

        /// <summary>
        /// Registers A course
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PATCTH /Register
        ///     {
        ///         Id = 1,
        ///         Title = "Generics In C#.",
        ///         CourseDescription = "Complete guide to generics in C#.",
        ///         Level = 1,
        ///         Price = 700,
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Details to update</param>
        /// <returns>An updated course</returns>
        /// <response code="200">If course updates successfully</response>
        /// <response code="400">If the item is null or if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        /// 


        [HttpPatch("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseToUpdateDTO model)
        {
            var course = await _courseRepo.Get(x => x.Id == model.Id);
            if (course is not null)
            {
                course = _mapper.Map<CourseToUpdateDTO, Course>(model, course);
                if (ModelState.IsValid)
                {
                    try
                    {
                        course.ModifiedOn = DateTime.Now;
                        var updatedCourse = await _courseRepo.Update(course);
                        return Ok(new ResponseModel("00", "Success", course));
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
                    }
                }
                else
                    return BadRequest(new ResponseModel("11", "There are some validation errors", ModelState.Values.SelectMany(e => e.Errors).ToList()));
            }
            return NotFound(new ResponseModel("22", "Course does not exist", null));

        }


        /// <summary>
        /// Deletes a course
        /// </summary>
        /// <param name="id">Course Id</param>
        /// <returns>null</returns>
        /// <response code="200">If course is deleted successfully</response>
        /// <response code="400">If the item is null or if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                var Course = await _courseRepo.Get(x => x.Id == id);
                await _courseRepo.Delete(Course);
                return Ok(new ResponseModel("00", "Success", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
            }
        }

        /// <summary>
        /// Fetches all registered courses
        /// </summary>
        /// <returns>All Registered courses</returns>
        /// <response code="200">If course fetcehed successfully</response>
        /// <response code="400">If the item is null or if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Getcourses()
        {
            try
            {
                var courses = await _courseRepo.GetAll();
                if (courses is not null && courses.Any()) return Ok(new ResponseModel("00", "Success", courses.Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.CourseDescription,
                    x.Level,
                    x.Price,
                })));
                return NotFound(new ResponseModel("22", "There are no Teachers in the database", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
            }

        }

        /// <summary>
        /// Gets a course with a specific Id
        /// </summary>
        /// <param name="id">Course ID</param>
        /// <returns>A course with the specified Id</returns>
        /// <response code="200">If course fetched successfully</response>
        /// <response code="400">If the item is null or if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourse(int id)
        {
            try
            {
                var course = await _courseRepo.Get(x => x.Id == id);
                if (course is not null) return Ok(new ResponseModel("00", "Success", new
                {
                    course.Id,
                    course.Title,
                    course.CourseDescription,
                    course.Level,
                    course.Price,
                }));
                return NotFound(new ResponseModel("22", "Teacher does not exist", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
            }
        }

        /// <summary>
        /// Fetchts all students that enrolled for a course
        /// </summary>
        /// <param name="id">Course id</param>
        /// <returns>A list of Students that offer a course</returns>
        /// <response code="200">If students are fetched successfully</response>
        /// <response code="400">if there are validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [HttpGet("{id:int}/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudents(int id)
        {
            try
            {
                var course = await _courseRepo.Get(x => x.Id == id);
                if (course is null)
                    return NotFound(new ResponseModel("22", "Course does not Exist", null));
                if (course.Students is not null && course.Students.Any())
                    return Ok(new ResponseModel("00", "Success", course.Students.Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.PhoneNumber,
                    })));

                return NotFound(new ResponseModel("22", "No Student has registerd for this course", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
            }
        }

        /// <summary>
        /// Fetchts all teachers that enrolled for a course
        /// </summary>
        /// <param name="id">Course Id</param>
        /// <returns>A list of teacers that offer a course</returns>
        /// <response code="200">If teachers are fetched successfully</response>
        /// <response code="400">If validation Errors</response> 
        /// <response code="404">If entity is null</response>
        [HttpGet("{id:int}/teachers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeachers(int id)
        {
            try
            {
                var course = await _courseRepo.Get(x => x.Id == id);
                if (course is null)
                    return NotFound(new ResponseModel("22", "Course does not Exist", null));
                if (course.Teachers is not null && course.Teachers.Any())
                    return Ok(new ResponseModel("00", "Success", course.Teachers.Select(x => new
                    {
                        x.FirstName,
                        x.LastName,
                        x.Email,
                        x.PhoneNumber,
                    })));

                return NotFound(new ResponseModel("22", "No Teacher has registerd for this course", null));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel("22", "Failed", new { ex.Message }));
            }
        }
    }
}
