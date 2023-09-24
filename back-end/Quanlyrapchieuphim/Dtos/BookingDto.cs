using Quanlyrapchieuphim.Models;

namespace Quanlyrapchieuphim.Dtos
{
    public class BookingDto
    {
        public Booking Booking { get; set; }
        public Dictionary<string, float> Seats { get; set; }
    }
}
