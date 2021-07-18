using RouletteTest.DBConnections;
using RouletteTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RouletteTest
{
    public class InternalProcess
    {
        private MongoDBConnection _DBProcess;
        public InternalProcess()
        {
            _DBProcess = new MongoDBConnection();
        }
        #region RouletteProcess
        public string CreateRoulette(string RouletteName)
        {
            Roulette roulette = new Roulette()
            {
                Id = GenerateRouletteId(),
                str_Name = RouletteName,
                bool_OpeningStatus = false
            };
            string ValidateResults = ValidateRouletteData(roulette);
            if (ValidateResults != "OK")
            {
                return ValidateResults;
            }
            _DBProcess.CreateRoulette(roulette);

            return "El ID de su nueva ruleta es: " + roulette.Id;
        }
        public string OpenRoulette(string RouletteId)
        {
            string ValidateResults = ValidateRouletteOpening(RouletteId);
            if (ValidateResults != "OK")
            {
                return ValidateResults;
            }
            _DBProcess.OpenRoulette(RouletteId);

            return "Operación aceptada. Ruleta abierta de forma exitosa.";
        }
        private string ValidateRouletteData(Roulette roulette)
        {
            if (string.IsNullOrEmpty(roulette.str_Name))
            {
                return "Debe ingresar un nombre para su ruleta.";
            }

            return "OK";
        }
        private string GenerateRouletteId()
        {
            Random random = new Random();
            byte[] buffer = new byte[24 / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            result = result.ToLower();

            return result;
        }
        private string ValidateRouletteOpening(string RouletteId)
        {
            if (string.IsNullOrEmpty(RouletteId))
            {
                return "Operación denegada. Debe ingresar el ID de alguna ruleta para abrirla.";
            }
            if (_DBProcess.FindRouletteForID(RouletteId.Trim()) == null)
            {
                return "Operación denegada. El ID que ingreso no pertenece a ninguna ruleta en el sistema.";
            }

            return "OK";
        }
        private string ValidateRouletteForBet(string RouletteId)
        {
            if (string.IsNullOrEmpty(RouletteId))
            {
                return "Operación denegada. Debe ingresar el ID de alguna ruleta para abrirla.";
            }
            if (_DBProcess.FindRouletteForID(RouletteId.Trim()) == null)
            {
                return "Operación denegada. El ID que ingreso no pertenece a ninguna ruleta en el sistema.";
            }
            if (!_DBProcess.FindStateRouletteForID(RouletteId.Trim()))
            {
                return "Operación denegada. La ruleta a la que intenta apostar no esta abierta.";
            }
            return "OK";
        }
        #endregion
        #region BetProcess
        public string CreateBet(Bet bet)
        {
            bet.str_RouletteId = bet.str_RouletteId.Trim();
            bet.str_ChoiceBet = bet.str_ChoiceBet.Trim().ToUpper();
            string ResponseValidates = ValidateRouletteForBet(bet.str_RouletteId);
            if (ResponseValidates != "OK")
            {
                return ResponseValidates;
            }
            ResponseValidates = ValidateDataBet(bet);
            if (ResponseValidates != "OK")
            {
                return ResponseValidates;
            }
            _DBProcess.CreateBet(bet);

            return "Operación aceptada. Apuesta creada de forma exitosa.";
        }
        public string ValidateDataBet(Bet bet)
        {
            if (string.IsNullOrEmpty(bet.str_ClientId))
            {
                return "Debe ingresar un ID de usuario valido para realizar la apuesta.";
            }
            if (string.IsNullOrEmpty(bet.str_ChoiceBet))
            {
                return "Debe elegir un número entre 0 o 36, o escoger ROJO o NEGRO para realizar su apuesta";
            }
            if (bet.double_BetAmount <= 0)
            {
                return "Debe apostar por montas mayores que cero (0)";
            }
            if (bet.double_BetAmount > 10000)
            {
                return "Cada apuesta no puede se mayore a 10.000 dólares en su monto.";
            }
            if (IsNumberValue(bet.str_ChoiceBet))
            {
                int ChoiseNumber = Convert.ToInt32(bet.str_ChoiceBet);
                if (ChoiseNumber < 0 || ChoiseNumber > 36)
                {
                    return "Los números disponibles a apostar son del 0 al 36";
                }
            }
            else
            {
                if (bet.str_ChoiceBet != "ROJO" && bet.str_ChoiceBet != "NEGRO")
                {
                    return "Solo puede ingresar las palabras Rojo o Negro para realizar la apuesta";
                }
            }

            return "OK";
        }
        #endregion


        private bool IsNumberValue(string value)
        {
            Regex regex = new Regex(@"^[0-9]+$");
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
            List<Bet> ParticipateList = _DBProcess.RouletteBettingList(RouletteId);
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
