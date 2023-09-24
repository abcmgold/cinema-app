namespace Quanlyrapchieuphim.Models
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public float TicketPrice { get; set; }
        public Guid BookingId { get; set; }
        public int Status { get; set; }
        public string FullName { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string Position { get; set; }
        public string MovieTitle { get; set; }
    }
}
