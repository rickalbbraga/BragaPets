using BragaPets.Domain.Enums;

namespace BragaPets.Domain.DTOs.Requests
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
    }
}