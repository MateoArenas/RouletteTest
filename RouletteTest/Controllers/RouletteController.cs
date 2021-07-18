using Microsoft.AspNetCore.Mvc;
using RouletteTest.DBConnections;
using RouletteTest.Models;
using System.Collections.Generic;

namespace RouletteTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private InternalProcess _InternalProcess;
        private MongoDBConnection _DBProcess;
        public RouletteController()
        {
            _InternalProcess = new InternalProcess();
            _DBProcess = new MongoDBConnection();
        }
        [HttpGet("[action]")]
        public ObjectResult ListRoulettes() 
        {
            try
            {
                return Ok(_DBProcess.ListRoulettes());
            }
            catch
            {
                return Ok("Tenemos un inconveniente al extraer la lista de las ruletas. Inténtalo más tarde.");
            }          
        }
        [HttpPost("[action]")]
        public ObjectResult CreateRoulette(string RouletteName)
        {
            try
            {
                return Ok(_InternalProcess.CreateRoulette(RouletteName));
            }
            catch
            {
                return Ok("Tenemos inconvenientes para crear su ruleta, inténtalo más tarde.");
            }
        }
        [HttpPut("[action]")]
        public ObjectResult OpenRoulette(string RouletteId)
        {
            try
            {               
                return Ok(_InternalProcess.OpenRoulette(RouletteId));
            }
            catch 
            {
                return Ok("Operación denegada. Inténtelo más tarde.");
            }
        }
        [HttpPut("[action]")]
        public ObjectResult CloseRoulette(string RouletteId)
        {
            try
            {
                _DBProcess.CloseRoulette(RouletteId);
                List<BettingResults> bettingResults = _InternalProcess.ListWinnersRoulette(RouletteId);
                //_DBProcess.DeleteBets(RouletteId);
                return Ok(bettingResults);
            }
            catch
            {
                return Ok("No hemos podido determinar los ganadores, inténtalo en otro momento.");
            }
        }
    }
}
