using Gifty.Data.Repositories;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;

namespace Gifty.Application.Services;

public class BirthdayReminderService
{
    //private readonly IUserRepository _userRepository; // Assuming you have a user repository to access users
    private readonly IUserRepository _userRepository;
    private readonly IFriendRequestRepository _friendRequestRepository; // Repository for friends
    private readonly EmailService _emailService;

    public BirthdayReminderService(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository, EmailService emailService)
    {
        _userRepository = userRepository;
        _friendRequestRepository = friendRequestRepository;
        _emailService = emailService;
    }

    public async Task CheckBirthdaysAsync(string userId)
    {
        // Get today's date
        var today = DateTime.UtcNow.Date;

        // Get accepted friends of the logged-in user
        var friends = await _friendRequestRepository.GetFriendsByUserIdAsync(userId);
        var upcomingBirthdays = new List<AppUser>(); // Using AppUser to get birthday

        foreach (var friend in friends)
        {
            // Create a birthday date for this year (ignoring the year part of the actual birthday)
            var friendBirthdayThisYear = new DateTime(today.Year, friend.Birthday.Month, friend.Birthday.Day);
            var birthdayDayOfYear = friendBirthdayThisYear.DayOfYear;

            // Adjust for birthdays in December/early January for year-overflow
            var daysUntilBirthday = birthdayDayOfYear - today.DayOfYear;
            if (daysUntilBirthday < 0) // For birthdays that have already passed this year
            {
                daysUntilBirthday += 365; // Adjust to check the next year (ignoring leap years here)
            }

            // Check if birthday is today or within the next 7 days
            if (daysUntilBirthday <= 7)
            {
                upcomingBirthdays.Add(friend);
            }
        }

        if (upcomingBirthdays.Any())
        {
            // Send notifications for upcoming birthdays
            var user = await _userRepository.GetByIdAsync(userId); // Fetch user details
            await SendBirthdayNotificationsAsync(user, upcomingBirthdays);
        }
    }




    private async Task SendBirthdayNotificationsAsync(AppUser user, List<AppUser> upcomingBirthdays)
    {
        foreach (var friend in upcomingBirthdays)
        {
            var subject = $"Birthday Reminder: {friend.FullName}'s birthday is coming up!";
            var body = $"Hi {user.FullName},\n\nJust a reminder that {friend.FullName}'s birthday is on {friend.Birthday.ToShortDateString()}!\n\nBest,\nYour Gifty App";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}


