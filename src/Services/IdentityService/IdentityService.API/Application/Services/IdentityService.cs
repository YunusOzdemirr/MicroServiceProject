using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IdentityService.API.Application.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.API.Application.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel)
        {
            //Db Process will be here. Check if user information is valid and get details
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,requestModel.UserName),
                new Claim(ClaimTypes.Name,"Yunus Özdemir")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a7zS9ka=SlsA8SJsma"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims, signingCredentials: creds, notBefore: DateTime.Now);
            var encodeJwt = new JwtSecurityTokenHandler().WriteToken(token);
            LoginResponseModel responseModel = new LoginResponseModel()
            {
                UserToken = encodeJwt,
                UserName = requestModel.UserName
            };
            return Task.FromResult(responseModel);
        }
    }
}

