using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Quanlyrapchieuphim.Models;
using System.Collections;
using System.Data;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : BaseController<Seat, Seat, Seat>
    {
        public SeatController(IConfiguration configuration) : base(configuration)
        {
        }
        [HttpGet("Filter")]
        public async Task<List<object>> GetSeatsFilter(Guid screenId, Guid screeningId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

            await mySqlConnection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@p_ScreenId", screenId);
            parameters.Add("@p_ScreeningId", screeningId);

            var seats = await mySqlConnection.QueryAsync<object>(
                sql: "Proc_Seat_Filter",
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            await mySqlConnection.CloseAsync();

            // Trả về danh sách các bản ghi kết quả từ procedure.
            return (List<object>)seats;
        }
    }
    
}
