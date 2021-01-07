using AutoMapper;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Exceptions;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Domain.Messages;
using UserCRUDApi.Domain.Validators;
using UserCRUDApi.Service.Interfaces;
using UserCRUDApi.Service.ViewModels;
using System;
using System.Linq;
using UserCRUDApi.Domain.Enums;
using System.Collections.Generic;

namespace UserCRUDApi.Service.Services
{
    public class UsuarioAppService : BaseAppService<Usuario, UsuarioViewModel, UsuarioValidator>, IUsuarioAppService
    {
        private readonly IMapper _mapper;

        public UsuarioAppService(IMapper mapper, IUnitOfWork unitOfWork, IService<Usuario> service)
            : base(mapper, unitOfWork, service)
        {
            _mapper = mapper;
        }

        public List<UsuarioViewModel> ListarUsuarios()
        {
            List<UsuarioViewModel> listaUsuarios = new List<UsuarioViewModel>();            

            foreach (var usuario in GetAll().ToList())
            {
                usuario.escolaridadeNome = ListarEscolaridades().Where(x => x.Id.Equals((int)usuario.Escolaridade)).FirstOrDefault().Nome;

                listaUsuarios.Add(usuario);
            }

            return listaUsuarios;
        }

        public UsuarioViewModel AdicionarComValidacao(UsuarioViewModel usuarioViewModel)
        {
            ExisteUsuarioComEsteEmail(usuarioViewModel.Email);
            ValidarDataNascimento(usuarioViewModel.DataNascimento);
            ValidarEscolaridade((int)usuarioViewModel.Escolaridade);

            UsuarioViewModel usuarioInsert = InsertOrUpdate(usuarioViewModel);

            if (!IsValidTransaction())
                throw new ServiceException(Messaging.MessageRepositoryError + Messaging.MessageSavedError);

            return _mapper.Map<UsuarioViewModel>(usuarioInsert);
        }

        public UsuarioViewModel AlterarComValidacao(UsuarioViewModel usuarioViewModel)
        {
            if (BuscarUsuarioComEsteId(usuarioViewModel.Id).Email != usuarioViewModel.Email)
                ExisteUsuarioComEsteEmail(usuarioViewModel.Email);

            ValidarDataNascimento(usuarioViewModel.DataNascimento);
            ValidarEscolaridade((int)usuarioViewModel.Escolaridade);

            UsuarioViewModel usuarioUpdate = InsertOrUpdate(usuarioViewModel);

            if (!IsValidTransaction())
                throw new ServiceException(Messaging.MessageRepositoryError + Messaging.MessageSavedError);

            return _mapper.Map<UsuarioViewModel>(usuarioUpdate);
        }

        public void ExcluirComValidacao(int id)
        {
            Delete(BuscarUsuarioComEsteId(id).Id);

            if (!IsValidTransaction())
                throw new ServiceException(Messaging.MessageRepositoryError + Messaging.MessageDeletedError);
        }

        public List<EscolaridadeViewModel> ListarEscolaridades()
        {
            return ((Escolaridade[])Enum.GetValues(typeof(Escolaridade))).Select(c => new EscolaridadeViewModel() {Id = (int)c, Nome = c.ToString()}).ToList();
        }

        private UsuarioViewModel BuscarUsuarioComEsteId(int id)
        {
            UsuarioViewModel usuario = _mapper.Map<UsuarioViewModel>(GetById(id));

            if (usuario == null)
                throw new ValidationException(Messaging.MessageRecordNotFound);

            return usuario;
        }

        private void ExisteUsuarioComEsteEmail(string email)
        {
            UsuarioViewModel usuarioEmail =  _mapper.Map<UsuarioViewModel>(GetAll()
                                                    .Where(e => e.Email.Equals(email))
                                                    .FirstOrDefault());

            if (usuarioEmail != null)
                throw new ValidationException(Messaging.MessageUserRegisteredWithThisEmail);
        }

        private void ValidarDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento > DateTime.Now.Date)
                throw new ValidationException(Messaging.MessageBirthDateError);
        }

        private void ValidarEscolaridade(int escolaridade)
        {
            var existeEscolaridade = ((Escolaridade[])Enum.GetValues(typeof(Escolaridade)))
                                                          .Select(c => (int)c).ToList().Contains(escolaridade);

            if (!existeEscolaridade)
                throw new ValidationException(Messaging.MessageEducationError);
        }
    }
}
