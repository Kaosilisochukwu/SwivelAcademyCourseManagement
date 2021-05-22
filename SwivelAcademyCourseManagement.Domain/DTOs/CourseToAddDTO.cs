using SwivelAcademyCourseManagement.Domain.Entities;
using SwivelAcademyCourseManagement.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SwivelAcademyCourseManagement.Domain.DTOs
{
    public class CourseToAddDTO
    {
        [Required(AllowEmptyStrings = false,ErrorMessage = "Title is Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course Description is Required")]
        [DisplayName("Course Description")]
        public string CourseDescription { get; set; }
        [Required(ErrorMessage = "Course level is required")]
        public Level Level { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Required(ErrorMessage = "Price Field is required")]
        public decimal Price { get; set; }
    }
}
