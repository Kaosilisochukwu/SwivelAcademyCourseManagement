using SwivelAcademyCourseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(3)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string CourseDescription { get; set; }
        [Required]
        public Level Level { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Teacher> Teachers { get; set; }
    }
}
