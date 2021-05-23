using SwivelAcademyCourseManagement.Data.Repository;
using SwivelAcademyCourseManagement.Domain.Models;

namespace SwivelAcademyCourseManagement.Data.Contracts
{
    public interface ITeacherRepository  : IBaseRepository<Teacher>, IUserRepository<Teacher>
    {       
    }
}
