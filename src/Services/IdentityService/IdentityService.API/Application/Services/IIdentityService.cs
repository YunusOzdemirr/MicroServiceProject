using System;
using IdentityService.API.Application.Models;

namespace IdentityService.API.Application.Services
{
	public interface IIdentityService
	{
		Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel);
	}
}

