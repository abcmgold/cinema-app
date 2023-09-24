using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Quanlyrapchieuphim.Models;
using static Dapper.SqlMapper;
using System.Data;
using Quanlyrapchieuphim.Dtos;
using System.Net.Mail;
using System.Net;

namespace Quanlyrapchieuphim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        protected readonly string? _connectionString;
        protected readonly IConfiguration? _configuration;
        public BookingController(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings"];
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<int> Booking([FromBody] BookingDto requestDTO)
        {
            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {
                var bookingId = Guid.NewGuid();
                var bookingParameters = new
                {
                    BookingId = bookingId,
                    ScreeningId = requestDTO.Booking.ScreeningId,
                    UserId = requestDTO.Booking.UserId,
                    BookingDate = requestDTO.Booking.BookingDate,
                    Amount = requestDTO.Booking.Amount,
                    PaymentMethod = requestDTO.Booking.PaymentMethod,
                    PaymentStatus = requestDTO.Booking.PaymentStatus
                };

                // Gọi procedure Pro_Booking_Addnew để thêm booking mới
                 var res = await connection.ExecuteAsync("Proc_Booking_AddNew", bookingParameters, commandType: CommandType.StoredProcedure);

                // Sau khi thêm booking thành công, tiến hành thêm vé cho mỗi seat
                foreach (var seat in requestDTO.Seats)
                {
                    var ticketParameters = new
                    {
                        TicketId = Guid.NewGuid(),
                        BookingId = bookingId,
                        TicketPrice = seat.Value, // Lấy giá trị giá vé từ 'seat.Value' trong Dictionary
                        SeatId = seat.Key, // Lấy giá trị SeatId từ 'seat.Key.SeatId' trong Dictionary
                        Status = 0 // mặc định là chưa nhận
                    };

                    // Gọi procedure Pro_Ticket_Addnew để thêm vé mới
                    await connection.ExecuteAsync("Proc_Ticket_AddNew", ticketParameters, commandType: CommandType.StoredProcedure);

                    var seatBookingParameters = new
                    {
                        SeatId = seat.Key,
                        ScreeningId = requestDTO.Booking.ScreeningId,
                        Status = 1
                    };
                    await connection.ExecuteAsync("Proc_SeatBooking_AddNew", seatBookingParameters, commandType: CommandType.StoredProcedure);
                }
                return res;
            }
        }

        [HttpPut]
        [Route("Change")]
        public async Task<int> ChangeStatus(Guid bookingId, int status)
        {

            using (var connection = new MySqlConnection(connectionString: _connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@p_BookingId", bookingId);
                parameters.Add("@p_Status", status);
                string storedProcedureName = $"Proc_Booking_UpdateStatus";

                int result = await connection.ExecuteAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        [HttpGet]
        public async Task<object> GetBookingById(Guid userId)
        {
            return 1;
        }
    }
}
