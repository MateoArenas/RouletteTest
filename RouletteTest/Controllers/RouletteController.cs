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
        private MongoDBConnection DBProcess;
        public RouletteController()
        {
            _generalProcess = new GeneralProcess();
            DBProcess = new MongoDBConnection();
        }
        [HttpGet("[action]")]
        public ObjectResult ListRoulettes() 
        {
            return Ok(DBProcess.ListRoulettes());
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
                DBProcess.CreateRoulette(roulette);
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
                DBProcess.OpenRoulette(Id);
                return "Operación aceptada";
            }
            catch 
            {
                return "Operación denegada";
            }
        }
    }
}
