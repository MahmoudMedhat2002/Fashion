using Fashion.Models;
using Fashion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
        {
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.Register(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}

		[HttpPost("token")]
		public async Task<IActionResult> GetToken(TokenRequestModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _authService.GetToken(model);

			if (!result.IsAuthenticated)
				return BadRequest(result.Message);

			return Ok(result);
		}

		[HttpGet("userId")]
		[Authorize(AuthenticationSchemes = "Bearer")]
		public IActionResult GetUserId()
		{
			var result = _authService.GetUserId();
			return Ok(result);
		}

		[HttpPost("addrole")]
		public async Task<IActionResult> AddToRole(AddRoleModel model)
		{
			var response = await _authService.AddToRole(model);
			return Ok(response);
		}
	}
}
