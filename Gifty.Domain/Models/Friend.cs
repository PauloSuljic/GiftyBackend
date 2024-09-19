namespace Gifty.Domain.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}