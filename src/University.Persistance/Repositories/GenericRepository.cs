using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.Contract;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        protected readonly UniversityDbContext _dbContext;

        public GenericRepository(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> CreateAsync(T entity)
        {

             await _dbContext.AddAsync(entity);
             //await _dbContext.SaveChangesAsync();
             return entity;
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.FindAsync<T>(id);
            if(entity != null)
            {
                _dbContext.Remove(entity);
                //await _dbContext.SaveChangesAsync();
                return entity;
            }
            else
            {
                throw new ArgumentNullException($"Entity with id {id} not found.");
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.FindAsync<T>(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Attach(entity).State = EntityState.Modified;
            //await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            return entity is not null;
        }
    }
}
