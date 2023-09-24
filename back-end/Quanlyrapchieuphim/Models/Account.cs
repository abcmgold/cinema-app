namespace Quanlyrapchieuphim.Models
{
    public class Account
    {
        public Guid AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
    }
}
