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

        public RouletteController()
        {
            _generalProcess = new GeneralProcess();
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
                _generalProcess.SaveRoulette(roulette);
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
                _generalProcess.OpenRoulette(Id);
                return "Operación aceptada";
            }
            catch 
            {
                return "Operación denegada";
            }
        }
    }
}
