using System;
using BragaPets.Domain.Entities;
using BragaPets.Domain.Enums;
using FluentValidation;

namespace BragaPets.Domain.Validations
{
    public class UserValidations : AbstractValidator<User>
    {
        public UserValidations()
        {
            RuleFor(x => x.Name)
                .MaximumLength(150)
                .WithMessage("Name must have maximum length of 150)");
            
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .WithMessage("Name must have minimum length of 3");
            
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email");
            
            RuleFor(x => x.Type)
                .Must(x => TypeValidate((int) x))
                .WithMessage("Invalid user type");
            
            RuleFor(x => x.Status)
                .Must(x => StatusValidate((int) x))
                .WithMessage("Invalid user status");
        }

        private bool TypeValidate(int type)
        {
           return Enum.IsDefined(typeof(UserType), type); 
        }
        
        private bool StatusValidate(int type)
        {
            return Enum.IsDefined(typeof(UserStatus), type); 
        }
    }
}