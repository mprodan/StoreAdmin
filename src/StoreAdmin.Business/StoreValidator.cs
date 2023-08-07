using FluentValidation;
using StoreAdmin.Core.Models;

namespace StoreAdmin.Business
{
    public class StoreValidator : AbstractValidator<Store>
    {
        public StoreValidator()
        {
            RuleFor(store => store.Name)
                .NotEmpty().WithMessage("Store name is required.")
                .MaximumLength(50).WithMessage("Store name 50 max.");
            RuleFor(store => store.Location)
                .NotEmpty().WithMessage("Store location is required.")
                .MaximumLength(50).WithMessage("Store location 50 max.");
        }
    }
}

