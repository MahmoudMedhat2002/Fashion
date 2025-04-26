using Fashion.Models;

namespace Fashion.Services
{
	public interface IAuthService
	{
		Task<AuthModel> Register(RegisterModel model);
		Task<AuthModel> GetToken(TokenRequestModel model);
		Task<string> AddToRole(AddRoleModel model);
		Task<AuthModel> RefreshTokenAsync(string refreshToken);
		Task<bool> RevokeRefreshToken(string refreshToken);
		string GetUserId();
	}
}
