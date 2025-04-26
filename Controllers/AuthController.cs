using Fashion.Models;
using Fashion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

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

            SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

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

			if (!string.IsNullOrEmpty(result.RefreshToken))
				SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

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
		[HttpGet("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
			var refreshToken = Request.Cookies["refreshToken"];
			var result = await _authService.RefreshTokenAsync(refreshToken);

			if(!result.IsAuthenticated)
				return BadRequest(result);

            SetRefreshTokenInCookies(result.RefreshToken , result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken(string? token)
        {
			var actualToken = token ?? Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(actualToken))
				return BadRequest("Token is required");

			var result = await _authService.RevokeRefreshToken(actualToken);

			if(!result)
				return BadRequest("Token is Invalid");

			return Ok(result);
        }

        private void SetRefreshTokenInCookies(string refreshToken , DateTime expires)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime()
			};

			Response.Cookies.Append("refreshToken", refreshToken , cookieOptions);
		}
	}
}
