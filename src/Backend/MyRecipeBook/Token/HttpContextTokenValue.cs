using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Token
{
    public class HttpContextTokenValue : ITokenProvider
    {
        readonly IHttpContextAccessor _contextAccessor;

        public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Value()
        {
            var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            return authentication["Bearer ".Length..].Trim();
        }
    }
}
