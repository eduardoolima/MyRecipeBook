using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Filters
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        readonly IAcessTokenValidator _acessTokenValidator;
        readonly IUserReadOnlyRepository _repository;

        public AuthenticatedUserFilter(IAcessTokenValidator acessTokenValidator, IUserReadOnlyRepository repository)
        {
            _acessTokenValidator = acessTokenValidator;
            _repository = repository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                string token = TokenOnRequest(context);
                var userIdentifier = _acessTokenValidator.ValidateAndGetUserIdentifier(token);                
                if (!await _repository.ExistActiveUserWithIdentifier(userIdentifier))
                {
                    throw new MyRecipeBookException(ResourceMessagesException.User_Without_Permission_Access);
                }
            }
            catch (MyRecipeBookException ex)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ex.Message));
            }
            catch (SecurityTokenExpiredException)
            {
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token Expirado")
                {
                    TokenIsExpired = true
                });
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult(ResourceMessagesException.User_Without_Permission_Access);
            }
        }

        static string TokenOnRequest(AuthorizationFilterContext context)
        {
            string authentication = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrWhiteSpace(authentication))
                throw new MyRecipeBookException(ResourceMessagesException.No_Token);

            return authentication["Bearer ".Length..].Trim();
        }
    }
}
