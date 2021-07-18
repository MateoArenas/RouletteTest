using Microsoft.AspNetCore.Mvc;

namespace RouletteTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private InternalProcess _InternalProcess;
        public RouletteController()
        {
            _InternalProcess = new InternalProcess();
        }
        [HttpGet("[action]")]
        public ObjectResult ListAllRoulettes() 
        {
            try
            {
                return Ok(_InternalProcess.ListRoulettes());
            }
            catch
            {
                return Ok("Tenemos un inconveniente al extraer la lista de las ruletas. Inténtalo más tarde.");
            }          
        }
        [HttpPost("[action]")]
        public ObjectResult CreateRoulette(string rouletteName)
        {
            try
            {
                return Ok(_InternalProcess.CreateRoulette(rouletteName: rouletteName));
            }
            catch
            {
                return Ok("Tenemos inconvenientes para crear su ruleta, inténtalo más tarde.");
            }
        }
        [HttpPut("[action]")]
        public ObjectResult OpenRoulette(string rouletteId)
        {
            try
            {               
                return Ok(_InternalProcess.OpenRoulette(rouletteId: rouletteId));
            }
            catch 
            {
                return Ok("Operación denegada. Inténtelo más tarde.");
            }
        }
        [HttpPost("[action]")]
        public ObjectResult CloseRoulette(string rouletteId)
        {
            try
            {               
                return Ok(_InternalProcess.CloseRoulette(rouletteId: rouletteId));
            }
            catch
            {
                return Ok("No hemos podido determinar los ganadores, inténtalo en otro momento.");
            }
        }
    }
}
