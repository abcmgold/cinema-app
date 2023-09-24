using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlyrapchieuphim.Models;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : BaseController<Cinema, Cinema, Cinema>
    {
        public CinemaController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
