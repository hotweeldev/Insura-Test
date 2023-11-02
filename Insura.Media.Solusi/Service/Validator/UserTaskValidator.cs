using FluentValidation;
using Insura.Media.Solusi.Common.Command;

namespace Insura.Media.Solusi.Service.Validator
{
    public class UserTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        public UserTaskValidator() 
        {
            RuleFor(x => x.TaskName)
                .NotEmpty();
            RuleFor(x => x.UserId)
                .NotEmpty();
            RuleFor(x => x.TaskModule)
                .NotEmpty();
        }
    }
}
