using System;
using System.ComponentModel.DataAnnotations;

namespace SwivelAcademyCourseManagement.Domain.Models
{
    public class BaseEntity<Pk>
    {
        [Key]
        public Pk Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
