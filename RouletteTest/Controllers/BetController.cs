using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("[action]")]
        public ObjectResult ListAllBets()
        {
            try
            {
                return Ok(_InternalProcess.ListBets());
            }
            catch
            {
                return Ok("Tenemos un inconveniente al extraer la lista de las apuestas. Inténtalo más tarde.");
            }
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
