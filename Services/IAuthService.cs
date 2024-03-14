using Fashion.Models;

namespace Fashion.Services
{
	public interface IAuthService
	{
		Task<AuthModel> Register(RegisterModel model);
		Task<AuthModel> GetToken(TokenRequestModel model);
		string GetUserId();
	}
}
