using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Service.Interfaces;
using UserCRUDApi.Service.ViewModels;
using UserCRUDApi.Domain.Exceptions;
using System.Net;
using UserCRUDApi.Presentation.Api.ViewModels;
using UserCRUDApi.Presentation.Api.Extensions;
using UserCRUDApi.Domain.Messages;

namespace UserCRUDApi.Presentation.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/usuario")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioAppService _appUsuarioService;        

        public UsuarioController(IUsuarioAppService appUsuarioService)
        {
            _appUsuarioService = appUsuarioService;            
        }

        [HttpGet()]
        [Route("listar")]
        [Authorize()]
        public ActionResult<dynamic> Listar()
        {
            try
            {
                var listaUsuarios = _appUsuarioService.ListarUsuarios();

                return Ok(listaUsuarios);
            }           
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,                    
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }

        [HttpGet()]
        [Route("{id}")]
        [Authorize()]
        public ActionResult<dynamic> GetById(int id)
        {
            try
            {
                var usuario = _appUsuarioService.GetById(id);

                return Ok(new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Data = usuario,
                    SuccessMessage = HttpStatusCode.OK.ToString()
                });
            }            
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,                    
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }

        /// <remarks>
        /// Escolaridade: 1 = Infantil; 2 = Fundamental; 3 = Médio; 4 = Superior
        /// </remarks>
        [HttpPost()]        
        [Authorize()]
        public ActionResult<dynamic> Adicionar([FromBody] UsuarioViewModel usuarioViewModel)
        {
            try
            {
                ModelValidate();                

                var usuarioService = _appUsuarioService.AdicionarComValidacao(usuarioViewModel);

                return Ok(new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Data = usuarioService,
                    SuccessMessage = Messaging.MessageSavedSuccess
                });
            }
            catch (ValidationException vex)
            {
                errors.Add(vex.Message);

                return StatusCode(HttpStatusCode.BadRequest.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = errors
                });

            }
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,                    
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }

        /// <remarks>
        /// Escolaridade: 1 = Infantil; 2 = Fundamental; 3 = Médio; 4 = Superior
        /// </remarks>
        [HttpPut()]        
        [Authorize()]
        public ActionResult<dynamic> Alterar([FromBody] UsuarioViewModel usuarioViewModel)
        {
            try
            {
                ModelValidate();

                var usuarioService = _appUsuarioService.AlterarComValidacao(usuarioViewModel);

                return Ok(new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Data = usuarioService,
                    SuccessMessage = Messaging.MessageSavedSuccess
                });
            }
            catch (ValidationException vex)
            {
                errors.Add(vex.Message);

                return StatusCode(HttpStatusCode.BadRequest.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = errors
                });

            }
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }

        [HttpDelete()]
        [Route("{id}")]
        [Authorize()]
        public ActionResult<dynamic> Excluir(int id)
        {
            try
            {
                _appUsuarioService.ExcluirComValidacao(id);

                return Ok(new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,                                        
                    SuccessMessage = Messaging.MessageDeletedSuccess
                });
            }
            catch (ValidationException vex)
            {
                errors.Add(vex.Message);

                return StatusCode(HttpStatusCode.BadRequest.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Errors = errors
                });

            }
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }

        [HttpGet()]
        [Route("escolaridade/listar")]
        [Authorize()]
        public ActionResult<dynamic> ListarEscolaridades()
        {
            try
            {
                var listaEscolaridades = _appUsuarioService.ListarEscolaridades();

                return Ok(listaEscolaridades);
            }
            catch (ServiceException servex)
            {
                errors.Clear();
                errors.Add(servex.Message);

                return StatusCode(HttpStatusCode.OK.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    Errors = errors
                });

            }
            catch (Exception ex)
            {
                errors.Clear();
                errors.Add(ex.Message);

                return StatusCode(HttpStatusCode.InternalServerError.ToInt(), new ResponseViewModel()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    Errors = errors
                });
            }
        }
    }
}