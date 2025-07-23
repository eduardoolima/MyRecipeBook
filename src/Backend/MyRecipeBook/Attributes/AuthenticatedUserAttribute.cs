using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Filters;

namespace MyRecipeBook.Attributes
{
    public class AuthenticatedUserAttribute : TypeFilterAttribute
    {
        public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter))
        {
        }
    }
}
