using Microsoft.IdentityModel.Tokens;
using StoreAdmin.Core.BusinessInterfaces;
using StoreAdmin.Core.Models;
using StoreAdmin.Core.RepositoryInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreAdmin.Business
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;

        public UserAuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string AuthenticateUser(string username, string password)
        {
            // Retrieve the user by username or email from the repository.
            var user = _userRepository.GetUserByusername(username);

            if (user == null)
            {
                return null;
            }

            // Verify the provided password with the hashed password from the database.
            if (!_userRepository.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(IUserAuthService.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Set the token expiration time.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
