using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : IAccessTokenGenerator
    {
        readonly uint _expirationInMinutes;
        readonly string _signingtKey;

        public JwtTokenGenerator(uint tokenExpirationInMinutes, string signingtKey)
        {
            _expirationInMinutes = tokenExpirationInMinutes;
            _signingtKey = signingtKey;
        }

        public string Generate(Guid userIdentifier)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Sid, userIdentifier.ToString())
                ]),
                Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
                SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        SymmetricSecurityKey SecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingtKey));
        }
    }
}
