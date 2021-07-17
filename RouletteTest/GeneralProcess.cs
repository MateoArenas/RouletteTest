using RouletteTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        public string ValidateDataBet(Bet bet)
        {
            if (string.IsNullOrEmpty(bet.str_ClientId))
            {
                return "Debe ingresar un Id de usuario valido para realizar la apuesta.";
            }
            if (string.IsNullOrEmpty(bet.str_Roulette))
            {
                return "Debe ingresar la ruleta a la que desea apostar.";
            }
            if (string.IsNullOrEmpty(bet.str_ChoiceBet))
            {
                return "Debe elegir un número entre 0 o 36, o escoger rojo o negro para realizar su apuesta";
            }
            if (bet.double_BetAmount <= 0)
            {
                return "Debe apostar por montas mayores que cero (0)";
            }
            return "OK";
        }
        public string ValidateMaxAmount(double Amount)
        {
            if (Amount > 10000)
            {
                return "Las apuestas no pueden ser mayores a 10.000 dólares";
            }
            return "OK";
        }
        public string ValidateChoiceBet(string choicebet)
        {
            if (IsNumberValue(choicebet))
            {
                int ChoiseNumber = Convert.ToInt32(choicebet);
                if (ChoiseNumber < 0 || ChoiseNumber > 36)
                {
                    return "Los números disponibles a apostar son del 0 al 36";
                }
            }
            else
            {
                if (choicebet.ToUpper() != "ROJO" && choicebet.ToUpper() != "NEGRO")
                {
                    return "Solo puede ingresar las palabras Rojo o Negro para realizar la apuesta";
                }
            }
            return "OK";
        }
        private bool IsNumberValue(string value)
        {
            Regex regex = new Regex(@"^\d$");
            return regex.IsMatch(value);
        }
        private string GenerateWinNumber()
        {
            Random random = new Random();
            int WinnerNumber = random.Next(0, 38);
            switch (WinnerNumber)
            {
                case 37:
                    return "NEGRO";
                case 38:
                    return "ROJO";
                default:
                    return WinnerNumber.ToString();
            }
        }
        public List<BettingResults> ListWinnersRoulette(string RouletteId)
        {
            List<BettingResults> ResultsList = new List<BettingResults>();
            List<Bet> ParticipateList = DBProcess.RouletteBettingList(RouletteId);
            string WinnerNumber = GenerateWinNumber();
            foreach (var item in ParticipateList)
            {
                if (WinnerNumber == item.str_ChoiceBet.ToUpper())
                {
                    if (IsNumberValue(WinnerNumber))
                    {
                        ResultsList.Add(new BettingResults
                        {
                            str_Client = item.str_ClientId,
                            str_Result = "Ha ganado en el juego de la ruleta!",
                            double_Profit = item.double_BetAmount * 5,
                        });
                    }
                    else
                    {
                        ResultsList.Add(new BettingResults
                        {
                            str_Client = item.str_ClientId,
                            str_Result = "Ha ganado en el juego de la ruleta!",
                            double_Profit = item.double_BetAmount * (1.8),
                        });
                    }
                }
                else
                {
                    ResultsList.Add(new BettingResults
                    {
                        str_Client = item.str_ClientId,
                        str_Result = "Ha perdido en el juego de la ruleta",
                        double_Profit = -item.double_BetAmount,
                    });
                }
            }
            return ResultsList;
        }
    }
}
