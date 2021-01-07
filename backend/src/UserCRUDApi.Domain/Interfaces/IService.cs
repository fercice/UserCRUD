using FluentValidation;
using System;
using System.Linq;
using System.Collections.Generic;
using UserCRUDApi.Domain.Entities;

namespace UserCRUDApi.Domain.Interfaces
{
    public interface IService<TEntity> where TEntity : Entity
    {
        TEntity Insert<V>(TEntity obj) where V : AbstractValidator<TEntity>;

        TEntity Update<V>(TEntity obj) where V : AbstractValidator<TEntity>;

        void Delete(int id);        

        IList<TEntity> GetAll();

        TEntity GetById(int id);

        IQueryable<TEntity> CreateQuery();
    }
}
