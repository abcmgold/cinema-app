namespace Quanlyrapchieuphim.Models
{
    public class Booking
    {
        public Guid BookingId { get; set; }
        public Guid ScreeningId { get; set; }
        public Guid UserId { get; set; }
        public DateTime BookingDate { get; set; }
        public float Amount { get; set; }
        public int? PaymentMethod { get; set; }
        public int PaymentStatus { get; set; }
    }
}
