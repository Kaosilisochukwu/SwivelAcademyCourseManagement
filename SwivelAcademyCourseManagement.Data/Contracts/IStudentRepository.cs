using SwivelAcademyCourseManagement.Data.Repository;
using SwivelAcademyCourseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Contracts
{
    public interface IStudentRepository : IBaseRepository<Student>, IUserRepository<Student>
    {
   
    }
}
