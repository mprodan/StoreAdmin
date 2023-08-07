using FluentValidation;
using StoreAdmin.Core.Models;

namespace StoreAdmin.Business
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username 50 max."); ;
            RuleFor(user => user.Email)
                .NotEmpty().EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(50).WithMessage("Email 50 max."); 
            RuleFor(user => user.PasswordHash).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}

