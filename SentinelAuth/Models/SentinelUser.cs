namespace SentinelAuth.Models
{
    public class SentinelUser
    {
        public Guid Id { get; set; }

        public required string Username { get; set; }

        public required string Role { get; set; }
    }
}