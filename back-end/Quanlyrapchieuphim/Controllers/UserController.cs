using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlyrapchieuphim.Models;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<User, User, User>
    {
        public UserController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
