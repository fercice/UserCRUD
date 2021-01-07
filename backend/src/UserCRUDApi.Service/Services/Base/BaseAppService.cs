using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Exceptions;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Service.Interfaces;
using UserCRUDApi.Service.ViewModels;
using System.Linq;

namespace UserCRUDApi.Service.Services
{
    public class BaseAppService<TEntity, VModel, Validator> 
        : IAppService<TEntity, VModel, Validator> where TEntity : Entity where VModel : BaseViewModel where Validator : AbstractValidator<TEntity>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService<TEntity> _service;

        public BaseAppService(IMapper mapper, IUnitOfWork unitOfWork, IService<TEntity> service)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _service = service;
        }

        public virtual VModel InsertOrUpdate(VModel viewModel)
        {
            try
            {
                return viewModel.Id == 0 ? Insert(viewModel) : Update(viewModel);
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }            
        }        

        public virtual void Delete(int id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }            
        }

        public virtual IEnumerable<VModel> GetAll()
        {
            try
            {
                return _mapper.Map<IEnumerable<TEntity>, IEnumerable<VModel>>(_service.GetAll());
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }            
        }

        public virtual VModel GetById(int id)
        {
            try
            {
                return _mapper.Map<VModel>(_service.GetById(id));
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }            
        }

        private VModel Insert(VModel viewModel)
        {
            try
            {                
                var insertCommand = _mapper.Map<TEntity>(viewModel);
                var resultCommand = _mapper.Map<VModel>(_service.Insert<Validator>(insertCommand));

                return resultCommand;
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }
        }

        private VModel Update(VModel viewModel)
        {
            try
            {
                var updateCommand = _mapper.Map<TEntity>(viewModel);
                _service.Update<Validator>(updateCommand);

                return viewModel;
            }
            catch (ServiceException e)
            {
                throw new ServiceException(e.Message);
            }
        }

        public IQueryable<TEntity> CreateQuery() => _service.CreateQuery();

        public bool IsValidTransaction() => _unitOfWork.Commit();

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
