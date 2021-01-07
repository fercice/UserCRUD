using System;
using System.Collections.Generic;
using System.Linq;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Infra.Data.Context;
using UserCRUDApi.Infra.Data.UoW;
using Microsoft.EntityFrameworkCore;

namespace UserCRUDApi.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly UnitOfWork _unitOfWork;        
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = (UnitOfWork)unitOfWork;
            _dbSet = context.Set<TEntity>();
        }

        protected UserCRUDContext context { get { return _unitOfWork._context; } }
        protected UserCRUDContext newcontext = new UserCRUDContext();

        public void Insert(TEntity obj)
        {
            _dbSet.Add(obj);
        }

        public void Update(TEntity obj)
        {
            _dbSet.Update(obj);
        }

        public void Delete(int id)
        {
            _dbSet.Remove(_dbSet.Find(id));
        }

        public IList<TEntity> GetAll()
        {
            return this.newcontext.Set<TEntity>().AsNoTracking().ToList();

        }

        public TEntity GetById(int id)
        {
            return this.newcontext.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> CreateQuery()
        {
            return this.newcontext.Set<TEntity>().AsNoTracking();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
