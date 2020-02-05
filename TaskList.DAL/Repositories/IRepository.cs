using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TaskList.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> GetById(int id);
        public Task<List<TEntity>> GetMultiple(Expression<Func<TEntity, bool>> filter);
        public Task Remove(TEntity toDelete);
        public Task<TEntity> RemoveById(int id);
        public Task Remove(IEnumerable<TEntity> entities);
        public Task Create(TEntity entity);
        public Task Update(TEntity entity);
    }
}