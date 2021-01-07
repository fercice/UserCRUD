using FluentValidation;
using System;
using System.Linq;
using System.Collections.Generic;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Interfaces;

namespace UserCRUDApi.Service.Services
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : Entity
    {
        private readonly IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public TEntity Insert<V>(TEntity obj) where V : AbstractValidator<TEntity>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repository.Insert(obj);
            return obj;
        }

        public TEntity Update<V>(TEntity obj) where V : AbstractValidator<TEntity>
        {
            Validate(obj, Activator.CreateInstance<V>());

            _repository.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public IList<TEntity> GetAll() => _repository.GetAll();

        public TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IQueryable<TEntity> CreateQuery() => _repository.CreateQuery();

        private void Validate(TEntity obj, AbstractValidator<TEntity> validator)
        {
            if (obj == null)
                throw new Exception("Dados nulos!");

            validator.ValidateAndThrow(obj);
        }
    }
}
