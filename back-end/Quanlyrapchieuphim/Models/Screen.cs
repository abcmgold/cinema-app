namespace Quanlyrapchieuphim.Models
{
    public class Screen
    {
        public Guid ScreenId { get; set; }
        public string ScreenName { get; set; }
        public Guid CinemaId { get; set; }
        public int SeatCount { get; set; }
        public Guid ScreenTypeId { get; set; }
        public string ScreenTypeName { get; set; }
        public int Status { get; set; }
    }
}
