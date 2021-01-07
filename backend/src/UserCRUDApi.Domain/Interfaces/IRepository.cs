using System;
using System.Collections.Generic;
using System.Linq;
using UserCRUDApi.Domain.Entities;

namespace UserCRUDApi.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Insert(TEntity obj);        

        void Update(TEntity obj);

        void Delete(int id);        

        IList<TEntity> GetAll();

        TEntity GetById(int id);

        IQueryable<TEntity> CreateQuery();

        int SaveChanges();
    }
}

