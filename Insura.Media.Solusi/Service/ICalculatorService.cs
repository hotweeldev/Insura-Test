using Insura.Media.Solusi.Common.Enums;
using Insura.Media.Solusi.Common.Query;

namespace Insura.Media.Solusi.Service
{
    public interface ICalculatorService
    {
        float Calculate(CalculatorQuery query);
    }
}
