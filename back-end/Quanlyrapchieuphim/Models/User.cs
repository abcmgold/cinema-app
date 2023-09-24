namespace Quanlyrapchieuphim.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public Guid AccountId { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}
