using FluentValidation;
using Insura.Media.Solusi.Common.Query;

namespace Insura.Media.Solusi.Service.Validator
{
    public class UserQueryValidator : AbstractValidator<UserQuery>
    {
        public UserQueryValidator() 
        {
            RuleFor(x => x.Page)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must start from 1.");
            RuleFor(x => x.Size)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Size minimum is 1.");
        }
    }
}
