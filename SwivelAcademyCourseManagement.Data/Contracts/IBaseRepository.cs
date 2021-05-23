using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Repository
{
    public interface IBaseRepository<T> where T : class, new()
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Expression<Func<T, bool>> query);
        Task Insert(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
    }
}
