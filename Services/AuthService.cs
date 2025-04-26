using Fashion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Fashion.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager , 
			IOptions<JWT> jwt , RoleManager<IdentityRole> roleManager , IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
			_roleManager = roleManager;
			_httpContextAccessor = httpContextAccessor;
			_jwt = jwt.Value;
        }

		public async Task<string> AddToRole(AddRoleModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserId);

			if (user is null)
				return "User Not Found";

			if (await _roleManager.FindByNameAsync(model.Role) is null)
				return $"There is no role with {model.Role} name";

			await _userManager.AddToRoleAsync(user, model.Role);

			return "Role added Success";
		}

		public async Task<AuthModel> GetToken(TokenRequestModel model)
		{
			var result = new AuthModel();

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user is null || !await _userManager.CheckPasswordAsync(user , model.Password))
			{
				result.Message = "Invalid Credentials";
				return result;
			}

			var jwtToken = await CreateJwtToken(user);
			var rolesList = await _userManager.GetRolesAsync(user);

			result.IsAuthenticated = true;
			result.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			result.Email = model.Email;
			//result.ExpiresOn = jwtToken.ValidTo;
			result.UserName = user.UserName;
			result.Roles = rolesList.ToList();

			if(user.RefreshTokens.Any(r => r.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
				result.RefreshToken = activeRefreshToken.Token;
				result.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
			else
			{
				var refreshToken = GenerateRefreshToken();
                result.RefreshToken = refreshToken.Token;
                result.RefreshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
            }

			return result;	
		}

		public string GetUserId()
		{
			return _httpContextAccessor.HttpContext.User.FindFirstValue("uid");
		}

        public async Task<AuthModel> RefreshTokenAsync(string refreshToken)
        {
			var authModel = new AuthModel();

			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

			if(user == null)
			{
				authModel.IsAuthenticated = false;
				authModel.Message = "Invalid Token";
				return authModel;
			}

			var token = user.RefreshTokens.Single(r => r.Token == refreshToken);

			if(!token.IsActive)
			{
                authModel.IsAuthenticated = false;
                authModel.Message = "Inactive Token";
                return authModel;
            }

			token.RevokedOn = DateTime.UtcNow;

			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);

			await _userManager.UpdateAsync(user);

			var jwtToken = await CreateJwtToken(user);

			return new AuthModel
			{
				IsAuthenticated = true,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
				Email = user.Email,
				UserName = user.UserName,
				Roles = (await _userManager.GetRolesAsync(user)).ToList(),
				RefreshToken = newRefreshToken.Token,
				RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };

        }

        public async Task<AuthModel> Register(RegisterModel model)
		{
			if(await _userManager.FindByEmailAsync(model.Email) is not null)
			{
				return new AuthModel { Message = "Email is already registered!!" };
			}

			if (await _userManager.FindByNameAsync(model.UserName) is not null)
			{
				return new AuthModel { Message = "User Name is already registered!!" };
			}

			var user = new ApplicationUser
			{
				UserName = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
			};

			var result = await _userManager.CreateAsync(user , model.Password);

			if (!result.Succeeded)
			{
				string errors = "";
				foreach(var error in result.Errors)
				{
					errors += $"{error.Description} ,";
				}

				return new AuthModel { Message = errors };
			}

			await _userManager.AddToRoleAsync(user, "User");

			var jwtToken = await CreateJwtToken(user);

			return new AuthModel
			{
				Email = user.Email,
				//ExpiresOn = jwtToken.ValidTo,
				IsAuthenticated = true,
				Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
				Roles = new List<string> { "User" },
				UserName = user.UserName,
			};
		}

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

			if (user == null)
				return false;

            var token = user.RefreshTokens.Single(r => r.Token == refreshToken);

			if (!token.IsActive)
				return false;

            token.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);

			return true;      
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = new List<Claim>();

			foreach (var role in roles)
				roleClaims.Add(new Claim("roles", role));

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}
			.Union(userClaims)
			.Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddDays(_jwt.DurationInDays),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var generator = new RNGCryptoServiceProvider();

			generator.GetBytes(randomNumber);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddDays(10),
				CreatedOn = DateTime.UtcNow
			};
		}
	}
}
