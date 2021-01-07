using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UserCRUDApi.Domain.Entities;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Infra.Data.Context;
using UserCRUDApi.Infra.Data.Repository;
using UserCRUDApi.Infra.Data.UoW;
using UserCRUDApi.Service.Interfaces;
using UserCRUDApi.Service.Services;

namespace UserCRUDApi.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // AppService
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();

            // Service                        
            services.AddScoped<IService<Usuario>, BaseService<Usuario>>();

            // Infra - Data  
            services.AddScoped<UserCRUDContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infra - Data - Repository            
            services.AddScoped<IRepository<Usuario>, Repository<Usuario>>();
        }
    }
}