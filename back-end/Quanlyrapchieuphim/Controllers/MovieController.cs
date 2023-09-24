using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Quanlyrapchieuphim.Models;
using System.Data;
using static Dapper.SqlMapper;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : BaseController<Movie, Movie, Movie>
    {
        public MovieController(IConfiguration configuration) : base(configuration)
        {

        }
        [HttpGet("Filter")]
        public async Task<List<Movie>> GetAsync(string? movieTitle, int? isRelease)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                await mySqlConnection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@MovieTitle", movieTitle);
                parameters.Add("@filterReleased", isRelease);

                var result = await mySqlConnection.QueryAsync<Movie>("Proc_Movie_UserFilter", parameters, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }


    }
}
