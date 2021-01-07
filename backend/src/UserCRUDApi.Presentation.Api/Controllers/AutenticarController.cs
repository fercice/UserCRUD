using UserCRUDApi.Presentation.Api.Extensions;
using UserCRUDApi.Presentation.Api.Authentication;
using UserCRUDApi.Presentation.Api.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using UserCRUDApi.Domain.Messages;
using UserCRUDApi.Domain.Interfaces;

namespace UserCRUDApi.Presentation.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/autenticar")]
    public class AutenticarController : BaseController
    {
        public AutenticarController(IUnitOfWork unitOfWork)
        {
        }
        
        [HttpPost]
        [Route("gerar-token")]
        public ActionResult<dynamic> GenerateToken()
        {
            try
            {
                return new
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    token = TokenService.GenerateToken(),
                    SuccessMessage = Messaging.MessageGenerateToken
                };
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
