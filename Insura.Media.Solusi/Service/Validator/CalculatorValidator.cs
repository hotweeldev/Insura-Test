using FluentValidation;
using Insura.Media.Solusi.Common.Query;

namespace Insura.Media.Solusi.Service.Validator
{
    public class CalculatorValidator : AbstractValidator<CalculatorQuery>
    {
        public CalculatorValidator() 
        {
            RuleFor(x => x.NumberOne)
                .NotEmpty();
            RuleFor(x => x.NumberTwo)
                .NotEmpty();
            RuleFor(x => x.AritmeticOperator)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
