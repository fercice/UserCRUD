using System.Collections.Generic;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Enums;
using UserCRUDApi.Domain.Validators;
using UserCRUDApi.Service.ViewModels;

namespace UserCRUDApi.Service.Interfaces
{
    public interface IUsuarioAppService : IAppService<Usuario, UsuarioViewModel, UsuarioValidator>
    {
        List<UsuarioViewModel> ListarUsuarios();

        UsuarioViewModel AdicionarComValidacao(UsuarioViewModel viewModel);

        UsuarioViewModel AlterarComValidacao(UsuarioViewModel viewModel);

        void ExcluirComValidacao(int id);

        List<EscolaridadeViewModel> ListarEscolaridades();
    }
}