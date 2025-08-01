﻿using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator
{
    public class JwtTokenGenerator : JwtTokenHandler, IAccessTokenGenerator
    {
        readonly uint _expirationInMinutes;
        readonly string _signingtKey;

        public JwtTokenGenerator(uint tokenExpirationInMinutes, string signingKey)
        {
            _expirationInMinutes = tokenExpirationInMinutes;
            _signingtKey = signingKey;
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
                SigningCredentials = new SigningCredentials(SecurityKey(_signingtKey), SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }       
    }
}
