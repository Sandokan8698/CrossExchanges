using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace XOProject
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class, new()
    {
        protected ExchangeContext _dbContext { get; set; }

        public async Task<T> GetAsync(string id)
        {
            return await _dbContext.FindAsync<T>(id);
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task InsertAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public Task<T> FindByIdAsync(object id)
        {
            return _dbContext.Set<T>().FindAsync(id);
        }


        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
                return _dbContext.Set<T>().Where(predicate).ToListAsync();
        }
    }
}