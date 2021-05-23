using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SwivelAcademyCourseManagement.Data.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {

        private readonly ApplicationDbContext context;
        private readonly DbSet<T> entities;
        string errorMessage = string.Empty;
        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async virtual Task<IEnumerable<T>> GetAll() => await entities.ToListAsync();
        public async virtual Task<T> Get(Expression<Func<T, bool>> query) =>
                    await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(query);

        public async virtual Task Insert(T entity)
        {
            if (entity == null)
            {
                var name = typeof(T).Name.Split('.').Last();
                errorMessage = $"{name} Does not Exist";
                throw new ArgumentNullException(errorMessage);
            }
            entities.Add(entity);
            await context.SaveChangesAsync();
        }
        public async Task<T> Update(T entity)
        {
            if (entity == null)
            {
                var name = typeof(T).Name.Split('.').Last();
                errorMessage = $"{name} Does not Exist";
                throw new ArgumentNullException(errorMessage);
            }
            entities.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                var name = typeof(T).Name.Split('.').Last();
                errorMessage = $"{name} Does not Exist";
                throw new ArgumentNullException(errorMessage);
            }
            entities.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
