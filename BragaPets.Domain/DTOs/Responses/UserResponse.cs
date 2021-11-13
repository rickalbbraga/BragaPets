using System;
using BragaPets.Domain.Enums;

namespace BragaPets.Domain.DTOs.Responses
{
    public class UserResponse
    {
        public Guid Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
        public UserStatus Status { get; set; }
        public bool FirstAccess { get; set; }
    }
}