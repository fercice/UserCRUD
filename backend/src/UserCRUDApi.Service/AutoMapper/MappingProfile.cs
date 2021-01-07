using AutoMapper;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Service.ViewModels;

namespace UserCRUDApi.Service.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // DomainToViewModel
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
        }
    }
}
