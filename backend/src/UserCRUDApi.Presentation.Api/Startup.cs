using AutoMapper;
using UserCRUDApi.Domain.Security;
using UserCRUDApi.Infra.CrossCutting.IoC;
using UserCRUDApi.Presentation.Api.Extensions;
using UserCRUDApi.Presentation.Api.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using System.Security.Claims;

namespace UserCRUDApi.Presentation.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {            
            var securityKey = Encoding.ASCII.GetBytes(Settings.SecretKey);

            services
                .AddMvc()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                    o.SerializerSettings.DateFormatString = "dd/MM/yyyy";
                });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddRouting(o => o.LowercaseUrls = true);

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("*");
            }));

            services.AddControllers();
            services.AddDistributedMemoryCache();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "UserCRUDApiApi",
                        Version = "v1",
                        Description = "API de Usuários",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "UserCRUDApiApi",
                            Url = new System.Uri("https://github.com/fercice/UserCRUDApiApi")
                        }
                    }
                );

                // Bearer token authentication
                Microsoft.OpenApi.Models.OpenApiSecurityScheme securityDefinition = new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition("Bearer", securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                Microsoft.OpenApi.Models.OpenApiSecurityScheme securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                    }
                };

                Microsoft.OpenApi.Models.OpenApiSecurityRequirement securityRequirements = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {securityScheme, new string[] { }},
                };
                c.AddSecurityRequirement(securityRequirements);

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "UserCRUDApi.xml");
                c.IncludeXmlComments(filePath);
            });

            // AddAutoMapper
            services.AddAutoMapperSetup();

            // .NET Native DI Abstraction
            RegisterServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserCRUDApiApi");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
