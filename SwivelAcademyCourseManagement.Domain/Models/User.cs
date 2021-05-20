using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IEnumerable<Course> Courses { get; set; }

    }
}
