using SwivelAcademyCourseManagement.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class Course : BaseEntity<int>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Course Description is Required")]
        [DisplayName("Course Description")]
        public string CourseDescription { get; set; }

        [Required(ErrorMessage = "Course level is required")]
        public Level Level { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Required(ErrorMessage = "Price Field is required")]
        public decimal Price { get; set; }

        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
