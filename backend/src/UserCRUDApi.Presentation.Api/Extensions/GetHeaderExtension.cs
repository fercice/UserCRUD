using UserCRUDApi.Presentation.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace UserCRUDApi.Presentation.Api.Extensions
{
    public static class GetHeaderExtension
    {
        public static JwtSecurityToken GetToken(this HttpRequest request)
        {
            var token = request.Headers["Authorization"].First().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }        
    }
}
