using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyRecipeBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MyRecipeBookBaseController : ControllerBase
    {
    }
}
