using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskList.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetById(int id)
        {
            var result = await _dbContext.Set<TEntity>().FindAsync(id)
                         ?? throw new DataException("Task not found!");
            return result;
        }

        public async Task<List<TEntity>> GetMultiple(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbContext.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task Remove(TEntity toDelete)
        {
            _dbContext.Set<TEntity>().Remove(toDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> RemoveById(int id)
        {
            var toDelete = await GetById(id);
            await Remove(toDelete);
            return toDelete;
        }

        public async Task Remove(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}