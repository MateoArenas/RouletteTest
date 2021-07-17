using Microsoft.AspNetCore.Mvc;
using RouletteTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        private GeneralProcess _generalProcess;
        private MongoDBConnection _DBProcess;
        public RouletteController()
        {
            _generalProcess = new GeneralProcess();
            _DBProcess = new MongoDBConnection();
        }
        [HttpGet("[action]")]
        public ObjectResult ListRoulettes() 
        {
            return Ok(_DBProcess.ListRoulettes());
        }
        [HttpPost("[action]")]
        public string CreateRoulette()
        {
            try
            {
                Roulette roulette = new Roulette()
                {
                    Id = _generalProcess.GenerateRouletteId(),
                    bool_OpeningStatus = false
                };
                _DBProcess.CreateRoulette(roulette);
                return "El id de su nueva ruleta es: " + roulette.Id;
            }
            catch
            {
                return "No logramas crear su ruleta, inténtalo más tarde.";
            }
        }
        [HttpPut("[action]")]
        public string OpenRouletteBets(string Id)
        {
            try
            {
                _DBProcess.OpenRoulette(Id);
                return "Operación aceptada";
            }
            catch 
            {
                return "Operación denegada";
            }
        }

        [HttpPut("[action]")]
        public ObjectResult CloseRouletteBets(string IdRoulette)
        {
            try
            {
                _DBProcess.CloseRoulette(IdRoulette);
                List<BettingResults> bettingResults = _generalProcess.ListWinnersRoulette(IdRoulette);
                _DBProcess.DeleteBets(IdRoulette);
                return Ok(bettingResults);
            }
            catch
            {
                return Ok("No hemos podido determinar los ganadores, inténtalo en otro momento.");
            }
        }
    }
}
