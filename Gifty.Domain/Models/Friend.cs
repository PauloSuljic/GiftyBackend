namespace Gifty.Domain.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string UserId { get; set; }
    }
}