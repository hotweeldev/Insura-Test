using Insura.Media.Solusi.Common.Enums;
using Insura.Media.Solusi.Common.Query;

namespace Insura.Media.Solusi.Service.Impl
{
    public class CalculatorServiceImpl : ICalculatorService
    {
        public float Calculate(CalculatorQuery query)
        {
            Dictionary<AritmeticOperator, Func<float, float, float>> operations = new Dictionary<AritmeticOperator, Func<float, float, float>>
            {
                { AritmeticOperator.Addition, (a, b) => a + b },
                { AritmeticOperator.Subtraction, (a, b) => a - b },
                { AritmeticOperator.Division, (a, b) => a / b },
                { AritmeticOperator.Multiplication, (a, b) => a * b }
            };

            if (operations.TryGetValue(query.AritmeticOperator, out var operation))
            {
                return operation(query.NumberOne, query.NumberTwo);
            }

            return 0;
        }

    }
}
