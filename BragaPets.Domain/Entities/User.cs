using BragaPets.Domain.DTOs.Requests;
using BragaPets.Domain.Enums;
using BragaPets.Domain.Validations;

namespace BragaPets.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public UserType Type { get; private set; }
        public UserStatus Status { get; private set; }
        public bool FirstAccess { get; private set; }

        private User()
        {
            
        }

        private User(string name, string email, UserType userType, UserStatus userStatus, bool firstAccess)
        {
            Name = name;
            Email = email;
            Type = userType;
            Status = userStatus;
            FirstAccess = firstAccess;

            Validate();
        }

        public void Update(UserUpdateRequest userRequest)
        {
            Name = userRequest.Name;
            Email = userRequest.Email;
            Type = userRequest.Type;
            Status = UserStatus.ACTIVE;
        }

        public static User Create(string name, string email, UserType userType, UserStatus userStatus, bool firstAccess)
            => new User(name, email, userType, userStatus, firstAccess);
        
        public static User Create(UserRequest userRequest)
            => new User(userRequest.Name, userRequest.Email, userRequest.Type, UserStatus.ACTIVE, true);

        private void Validate()
        {
            var validator = new UserValidations().Validate(this);
            
            if (!validator.IsValid)
                Errors = validator.Errors;
        }
    }
}