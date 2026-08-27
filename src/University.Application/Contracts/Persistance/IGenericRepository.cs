using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;

namespace University.Application.Contracts.Persistance
{
    public interface IGenericRepository<T> where T : IBaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdDetailAsync(Guid id);
        Task<T> CreateAsync(T entity);
        T Update(T entity);
        Task<T> DeleteAsync(Guid id);

        Task<bool> ExistsAsync(Guid id);
    }
}
