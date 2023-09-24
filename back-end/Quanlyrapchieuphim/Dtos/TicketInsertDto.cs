namespace Quanlyrapchieuphim.Dtos
{
    public class TicketInsertDto
    {
        public Guid TicketId { get; set; }
        public Guid SeatId { get; set; }
        public float TicketPrice { get; set; }
        public Guid BookingId { get; set; }
        public int Status { get; set; }
    }
}
