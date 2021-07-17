using Microsoft.AspNetCore.Mvc;
using RouletteTest.Model;

namespace RouletteTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private GeneralProcess _generalProcess;
        private MongoDBConnection _DBProcess;
        public BetController()
        {
            _generalProcess = new GeneralProcess();
            _DBProcess = new MongoDBConnection();
        }

        [HttpPost("[action]")]
        public string CreateBet(Bet bet)
        {
            try
            {
                string ResponseValidates = _generalProcess.ValidateDataBet(bet);
                if (ResponseValidates != "OK")
                {
                    return ResponseValidates;
                }
                ResponseValidates = _generalProcess.ValidateMaxAmount(bet.double_BetAmount);
                if (ResponseValidates != "OK")
                {
                    return ResponseValidates;
                }
                ResponseValidates = _generalProcess.ValidateChoiceBet(bet.str_ChoiceBet);
                if (ResponseValidates != "OK")
                {
                    return ResponseValidates;
                }
                _DBProcess.CreateBet(bet);
                return "Apuesta ingresada exitosamente.";
            }
            catch
            {
                return "No podemos ingresar su apuesta, inténtelo más tarde.";
            }
        }
    }
}
