using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Contract;

namespace University.Application.Services.Contract
{
    public interface IGenericService<TEntity, TCreateDto, TGetDto, in TUpdateDto> where TEntity:IBaseEntity 
        where TCreateDto: ICreateDto
        where TGetDto: IQueryDto
        where TUpdateDto : IUpdateDto
    {
        Task<List<TGetDto>> GetAllAsync();
        Task<TGetDto?> GetByIdAsync(Guid id);
        Task<TGetDto> CreateAsync(TCreateDto dto);
        Task<TGetDto> UpdateAsync(TUpdateDto dto, Guid id);
        Task<TGetDto> DeleteAsync(Guid id, Staff deletedBy);

    }
}
