using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Quanlyrapchieuphim.Dtos;
using Quanlyrapchieuphim.Models;
using static Dapper.SqlMapper;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : BaseController<Ticket, Ticket, TicketInsertDto>
    {
        public TicketController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpPut]
        [Route("Change")]
        public async Task<int> ChangeStatus(Guid ticketId, int status)
        {

            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_TicketId", ticketId);
                parameters.Add("@p_Status", status);
                string storedProcedureName = $"Proc_Ticket_UpdateStatus";

                int result = await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        [HttpGet]
        [Route("GetByBookingId")]
        public async Task<List<Ticket>> GetByBookingId(string? bookingId)
        {

            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@bookingId", bookingId);
                string storedProcedureName = "Proc_Ticket_FilterByBookingId";

                var result = await connection.QueryAsync<Ticket>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return (List<Ticket>)result;
            }
        }
    }
}
