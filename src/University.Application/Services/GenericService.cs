using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Contracts.Persistance;
using University.Application.Services.Contract;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Contract;

namespace University.Application.Services
{
    public abstract class GenericService<TEntity, TCreateDto, TGetDto, TUpdateDto> : IGenericService<TEntity, TCreateDto, TGetDto, TUpdateDto> 
        where TEntity:IBaseEntity
        where TCreateDto : ICreateDto
        where TGetDto : IQueryDto
        where TUpdateDto : IUpdateDto
    {
        private readonly IGenericRepository<TEntity> _repository;
        protected GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
        }

        protected abstract TGetDto ToGetDto(TEntity entity);
        protected abstract TEntity ToEntity(TCreateDto dto, Staff createdBy);
        protected abstract TEntity ApplyUpdate(TEntity entity, TUpdateDto dto, Staff updatedBy);

        public async Task<TGetDto> CreateAsync(TCreateDto dto, Staff createdBy)
        {
            try
            {
                var entityToBeCreated = ToEntity(dto, createdBy);
                await _repository.CreateAsync(entityToBeCreated);
                return ToGetDto(entityToBeCreated);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(TEntity)} can not be created", ex);
            }

        }

        public async Task<TGetDto> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _repository.DeleteAsync(id);
                return ToGetDto(entity);
            }
            catch(Exception ex)
            {
                throw new Exception($"{nameof(TEntity)} with id {id} can not be deleted", ex);
            }

        }

        public async Task<List<TGetDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return entities.Select(ToGetDto).ToList();
            }
            catch(Exception ex)
            {
                throw new Exception($"Problem with fetching {nameof(TEntity)}", ex);
            }

        }

        public async Task<TGetDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if(entity is not null)
            {
                return ToGetDto(entity);
            }
            throw new Exception($"Problem with fetching {nameof(TEntity)} with id {id}");
        }

        public async Task<TGetDto> UpdateAsync(TUpdateDto dto, Guid id, Staff updatedBy)
        {
            try
            {
                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity is null)
                {
                    throw new Exception("No Entity found");
                }
                ApplyUpdate(existingEntity, dto, updatedBy);
                await _repository.UpdateAsync(existingEntity);
                return ToGetDto(existingEntity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Problem with updating {nameof(TEntity)} with id {id}", ex);
            }


        }
    }
}
