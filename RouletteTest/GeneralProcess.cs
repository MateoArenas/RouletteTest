using RouletteTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteTest
{
    public class GeneralProcess
    {
        private MongoDBConnection DBProcess;

        public GeneralProcess()
        {
            DBProcess = new MongoDBConnection();
        }

        public string GenerateRouletteId()
        {
            Random random = new Random();
            byte[] buffer = new byte[24 / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            result = result.ToLower();
            return result;
        }

        public void SaveRoulette(Roulette roulette) 
        {
            DBProcess.CreateRoulette(roulette);
        }

        public void OpenRoulette(string id) 
        {
            DBProcess.OpenRoulette(id);
        }
    }
}
