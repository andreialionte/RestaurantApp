﻿namespace RestaurantApp.DTOs
{
    public class LoginConfirmationDto
    {
        public string? Email { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }
    }
}
