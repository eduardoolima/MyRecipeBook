using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access
{
    public abstract class JwtTokenHandler
    {
        protected SymmetricSecurityKey SecurityKey(string signingKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }
    }
}
