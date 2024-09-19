namespace Gifty.Domain.Models
{
    public class BirthdayReminder
    {
        public int Id { get; set; }
        public DateTime ReminderDate { get; set; }
        public int FriendId { get; set; }
        public Friend Friend { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}