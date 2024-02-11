using Microsoft.AspNetCore.Mvc;

namespace MobileTopup.Balance.Api.Controllers
{
    public class BalanceController : Controller
    {
        [HttpGet("getBalance")]
        public async Task<IActionResult> GetBalance(Guid userId)
        {
            Random random = new Random();
            
            long result = random.NextInt64(1, 5000);

            return Ok(result);
        }

        [HttpPost("executeDebit")]
        public async Task<IActionResult> ExecuteDebit([FromBody] Guid userId, long amount)
        {

            return Ok(true);
        }
    }
}
