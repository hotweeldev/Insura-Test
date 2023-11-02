using FluentValidation;
using Insura.Media.Solusi.Common.Command;

namespace Insura.Media.Solusi.Service.Validator
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator() 
        {
            RuleFor(x => x.Name)
                .MaximumLength(25);
            RuleFor(x => x.Address)
                .MaximumLength(100);
            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
