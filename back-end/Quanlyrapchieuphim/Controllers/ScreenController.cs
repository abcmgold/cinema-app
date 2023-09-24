using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlyrapchieuphim.Models;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreenController : BaseController<Screen, Screen, Screen>
    {
        public ScreenController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
