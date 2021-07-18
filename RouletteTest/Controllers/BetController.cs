using Microsoft.AspNetCore.Mvc;
using RouletteTest.DBConnections;
using RouletteTest.Models;

namespace RouletteTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private InternalProcess _InternalProcess;
        public BetController()
        {
            _InternalProcess = new InternalProcess();
        }
        [HttpPost("[action]")]
        public ObjectResult CreateBet(Bet bet)
        {
            try
            {             
                return Ok(_InternalProcess.CreateBet(bet));
            }
            catch
            {
                return Ok("No podemos ingresar su apuesta, inténtelo más tarde.");
            }
        }
    }
}
