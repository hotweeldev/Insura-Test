using Insura.Media.Solusi.Common.Query;
using Insura.Media.Solusi.Service;
using Microsoft.AspNetCore.Mvc;

namespace Insura.Media.Solusi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService calculatorService;
        public CalculatorController(ICalculatorService calculatorService)
        {
            this.calculatorService = calculatorService;
        }

        [HttpGet]
        public IActionResult GetCalculation([FromQuery] CalculatorQuery query)
        {
            return Ok(calculatorService.Calculate(query));
        }
    }
}
