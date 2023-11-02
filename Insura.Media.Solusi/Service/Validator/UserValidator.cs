using FluentValidation;
using Insura.Media.Solusi.Common.Command;

namespace Insura.Media.Solusi.Service.Validator
{
    public class UserValidator : AbstractValidator<CreateUserCommand>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(25);
            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.NIK)
                .NotEmpty()
                .MaximumLength(16)
                .MinimumLength(16)
                .WithMessage("Length of NIK must 16 character");
            RuleFor(x => x.Status)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
