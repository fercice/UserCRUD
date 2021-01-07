using FluentValidation;
using System;
using System.Collections.Generic;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Service.ViewModels;

namespace UserCRUDApi.Service.Interfaces
{
    public interface IAppService<TEntity, VModel, Validator>
        where TEntity : Entity
        where VModel : BaseViewModel 
        where Validator : AbstractValidator<TEntity>
    {        
        VModel InsertOrUpdate(VModel vmodel);        

        void Delete(int id);

        IEnumerable<VModel> GetAll();

        VModel GetById(int id);
    }
}

