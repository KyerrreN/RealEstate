﻿namespace NotificationService.Contracts
{
    public class UserRegisteredEvent
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
    }
}
