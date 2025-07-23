using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Attributes;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Controllers
{    
    public class UserController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody]RequestRegisterUserJson request, [FromServices]IRegisterUserUseCase useCase)
        {            
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);//comentario teste
        }
    }
}