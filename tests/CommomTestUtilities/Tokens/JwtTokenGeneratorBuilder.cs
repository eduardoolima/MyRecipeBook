using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;

namespace CommomTestUtilities.Tokens
{
    public class JwtTokenGeneratorBuilder
    {
        public static IAccessTokenGenerator Build() => new JwtTokenGenerator(tokenExpirationInMinutes: 5, signingKey: "ttttttttttttttttttttttttttttttttttttttttt");
    }
}
