using Microsoft.AspNetCore.Mvc;
using Quanlyrapchieuphim.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : BaseController<Genre, Genre, Genre>
    {
        public GenreController(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
