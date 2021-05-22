using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class User : BaseEntity<string>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        [MaxLength(50, ErrorMessage = "First Name must not be longer than 50 characters")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        [MaxLength(50, ErrorMessage = "Last Name must not be longer than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Adress is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email field must be a valid email Address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone Number field is required")]
        [DisplayName("Phone Number")]
        [MaxLength(15, ErrorMessage = "Phone Number must not be longer than 15 characters")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone Number must be a valid phone number")]
        public string PhoneNumber { get; set; }

        public List<Course> Courses { get; set; }

    }
}
