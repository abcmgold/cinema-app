using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Quanlyrapchieuphim.Models;
using System.Data;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreeningController : BaseController<Screening, Screening, Screening>
    {
        public ScreeningController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet(template: "Filter")]
        public async Task<List<Screening>> GetScreeningFilter(DateTime date, string cinemaName)
        {
            MySqlConnection mySqlConnection = new MySqlConnection(_connectionString);

            await mySqlConnection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@p_Date", date, DbType.Date);
            parameters.Add("@p_CinemaName", cinemaName);

            var screenings = await mySqlConnection.QueryAsync<Screening>(
                sql: "Proc_Screening_Filter", // Thay thế tên procedure thực tế nếu cần.
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            await mySqlConnection.CloseAsync();

            // Trả về danh sách các bản ghi kết quả từ procedure.
            return (List<Screening>)screenings;
        }

        [HttpPost]
        public override async Task<ActionResult> InsertAsync(Screening screening)
        {
            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {


                var parameters = new DynamicParameters();
                parameters.Add("@timeStart", screening.TimeStart);
                parameters.Add("@timeEnd", screening.TimeEnd);
                string storedProcedureName = "Proc_Screening_FilterByTime";

                var result = await connection.QueryFirstOrDefaultAsync<Screening>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    return BadRequest("Phòng chiếu đã được sử dụng trong thời gian này");
                }

                var guid = Guid.NewGuid();
                parameters.Add("@p_ScreeningId", guid);

                foreach (var prop in screening.GetType().GetProperties())
                {
                    if (string.Equals(prop.Name, "ScreeningId"))
                    {
                        continue;
                    }
                    parameters.Add("@p_" + prop.Name, prop.GetValue(screening, null));
                }

                storedProcedureName = "Proc_Screening_AddNew";

                var res = await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return Ok(res);
            }
        }
        [HttpGet]
        [Route("AdminFilterByName")]
        public async Task<List<Screening>> GetFilterAsync(string? name)
        {
            var parameters = new DynamicParameters();
            parameters.Add("movieTitle", name);

            using (MySqlConnection mySqlConnection = new MySqlConnection(_connectionString))
            {
                await mySqlConnection.OpenAsync();
                var result = await mySqlConnection.QueryAsync<Screening>($"CALL Proc_Screening_AdminFilter(@movieTitle)", parameters);
                return (List<Screening>)result;
            }
        }
    }
}
