using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public class BirthdayReminderService
    {
        private readonly IUserRepository _userRepository; // User repository for accessing user details
        private readonly IFriendRequestRepository _friendRequestRepository; // Repository for managing friend requests
        private readonly EmailService _emailService;

        public BirthdayReminderService(IUserRepository userRepository, IFriendRequestRepository friendRequestRepository, EmailService emailService)
        {
            _userRepository = userRepository;
            _friendRequestRepository = friendRequestRepository;
            _emailService = emailService;
        }

        // Check upcoming birthdays for friends of the logged-in user
        public async Task CheckBirthdaysAsync(string userId)
        {
            // Get today's date
            var today = DateTime.UtcNow.Date;

            // Fetch confirmed friends (accepted friend requests)
            var confirmedFriendRequests = await _friendRequestRepository.GetConfirmedRequestsForUserAsync(userId);
            var upcomingBirthdays = new List<AppUser>();

            // Loop through each confirmed friend request and get the friend's details
            foreach (var friendRequest in confirmedFriendRequests)
            {
                // Get the friend's userId (opposite of the logged-in user's ID)
                var friendUserId = friendRequest.SenderId == userId ? friendRequest.ReceiverId : friendRequest.SenderId;

                // Fetch friend's details from the user repository
                var friend = await _userRepository.GetByIdAsync(friendUserId);
                
                if (friend != null && friend.Birthday != null) // Ensure friend and birthday exist
                {
                    var friendBirthdayThisYear = new DateTime(today.Year, friend.Birthday.Month, friend.Birthday.Day);

                    // Calculate the number of days until the birthday
                    var daysUntilBirthday = (friendBirthdayThisYear - today).Days;

                    // Adjust for birthdays that have already passed this year
                    if (daysUntilBirthday < 0)
                    {
                        daysUntilBirthday += 365;
                    }

                    // Check if the birthday is today or within the next 7 days
                    if (daysUntilBirthday <= 7)
                    {
                        upcomingBirthdays.Add(friend);
                    }
                }
            }

            // If there are upcoming birthdays, send notifications
            if (upcomingBirthdays.Any())
            {
                var user = await _userRepository.GetByIdAsync(userId); // Fetch logged-in user details
                await SendBirthdayNotificationsAsync(user, upcomingBirthdays);
            }
        }

        // Helper method to send birthday notifications via email
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
}
