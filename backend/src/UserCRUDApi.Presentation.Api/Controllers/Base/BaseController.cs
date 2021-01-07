using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Presentation.Api.ViewModels;
using UserCRUDApi.Presentation.Api.Extensions;
using System;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using UserCRUDApi.Domain.Exceptions;
using UserCRUDApi.Domain.Messages;

namespace UserCRUDApi.Presentation.Api.Controllers
{
    public class BaseController : Controller
    {        
        public List<string> errors = new List<string>();

        [ApiExplorerSettings(IgnoreApi = true)]
        public void ModelValidate()
        {
            errors.Clear();            

            if (!ModelState.IsValid)
            {
                foreach (var modelStateVal in ViewData.ModelState.Values)
                    foreach (var error in modelStateVal.Errors)
                        errors.Add(error.ErrorMessage);

                if (errors.Count > 0)
                    throw new ValidationException(Messaging.MessageRequiredFields);
            }
        }
    }
}
