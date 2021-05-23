using SwivelAcademyCourseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Domain.DTOs
{
    public class CourseToUpdateDTO
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is Required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Course Description is Required")]
        [DisplayName("Course Description")]
        public string CourseDescription { get; set; }
        [Required(ErrorMessage = "Course level is required")]
        public Level Level { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 1")]
        [Required(ErrorMessage = "Price Field is required")]
        public decimal Price { get; set; }
    }
}
