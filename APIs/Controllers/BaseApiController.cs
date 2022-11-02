using APIs.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

        
    }
}