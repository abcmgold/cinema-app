namespace Quanlyrapchieuphim.Models
{
    /// <summary>
    /// Suất chiếu
    /// </summary>
    public class Screening
    {
        public Guid ScreeningId { get; set; }
        public Guid MovieId { get; set; }
        public Guid CinemaId { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public Guid ScreenId { get; set; }
        public decimal TicketPrice { get; set; }
        public int Status { get; set; }
        public string? MovieTitle { get; set; }
        public string? ScreenName { get; set; }
        public string? CinemaName { get; set; }
    }
}
