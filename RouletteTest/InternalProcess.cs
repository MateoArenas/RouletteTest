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
        public List<Roulette> ListRoulettes()
        {
            return _DBProcess.ListAllRoulettes();
        }
        public string CreateRoulette(string rouletteName)
        {
            Roulette roulette = new Roulette()
            {
                Id = GenerateRouletteId(),
                str_Name = rouletteName,
                bool_OpeningStatus = false
            };
            string ValidateResults = ValidateRouletteData(roulette);
            if (ValidateResults != "OK")

                return ValidateResults;
            _DBProcess.CreateRoulette(roulette);

            return "El ID de su nueva ruleta es: " + roulette.Id;
        }
        public string OpenRoulette(string rouletteId)
        {
            string ValidateResults = ValidateRouletteOpening(rouletteId: rouletteId);
            if (ValidateResults != "OK")

                return ValidateResults;
            _DBProcess.OpenRoulette(rouletteId);

            return "Operación aceptada. Ruleta abierta de forma exitosa.";
        }
        public List<BettingResults> CloseRoulette(string rouletteId)
        {
            List<BettingResults> bettingResults = new List<BettingResults>();
            if (!_DBProcess.FindStateRouletteForID(rouletteId: rouletteId))
            {
                bettingResults.Add(new BettingResults()
                {
                    str_Client = "00000",
                    str_Result = "No hay resultados para esta ruleta ya que se encuentra cerrada.",
                    double_Profit = 0
                });

                return bettingResults;
            }
            _DBProcess.CloseRoulette(rouletteId: rouletteId);
            bettingResults = GameResultsList(rouletteId: rouletteId);
            _DBProcess.DeleteBetsForRuletteId(rouletteId: rouletteId);
            return bettingResults;
        }
        private string ValidateRouletteData(Roulette roulette)
        {
            if (string.IsNullOrEmpty(roulette.str_Name))

                return "Debe ingresar un nombre para su ruleta.";

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
        private string ValidateRouletteOpening(string rouletteId)
        {
            if (string.IsNullOrEmpty(rouletteId))

                return "Operación denegada. Debe ingresar el ID de alguna ruleta para abrirla.";
            if (_DBProcess.FindRouletteForID(rouletteId: rouletteId.Trim()) == null)

                return "Operación denegada. El ID que ingreso no pertenece a ninguna ruleta en el sistema.";

            return "OK";
        }
        private string ValidateRouletteForBet(string rouletteId)
        {
            if (string.IsNullOrEmpty(rouletteId))

                return "Operación denegada. Debe ingresar el ID de alguna ruleta para realizar su apuesta.";
            if (_DBProcess.FindRouletteForID(rouletteId: rouletteId.Trim()) == null)

                return "Operación denegada. El ID que ingreso no pertenece a ninguna ruleta en el sistema.";
            if (!_DBProcess.FindStateRouletteForID(rouletteId: rouletteId.Trim()))

                return "Operación denegada. La ruleta a la que intenta apostar no esta abierta.";

            return "OK";
        }
        private WinnerResult GenerateWinningResult()
        {
            WinnerResult winnerResult = new WinnerResult();
            Random random = new Random();
            int WinnerNumber = random.Next(37);
            if (WinnerNumber == 0)
            {
                winnerResult.str_WinnerNumber = WinnerNumber.ToString();
                winnerResult.str_WinnerColor = "ROJO";
            }
            else
            {
                if (WinnerNumber % 2 == 0)
                {
                    winnerResult.str_WinnerNumber = WinnerNumber.ToString();
                    winnerResult.str_WinnerColor = "ROJO";
                }
                else
                {
                    winnerResult.str_WinnerNumber = WinnerNumber.ToString();
                    winnerResult.str_WinnerColor = "NEGRO";
                }
            }

            return winnerResult;
        }
        private List<BettingResults> GameResultsList(string rouletteId)
        {
            List<BettingResults> ResultsList = new List<BettingResults>();
            List<Bet> BettingList = _DBProcess.BettingListForRouletteId(rouletteId: rouletteId);
            WinnerResult WinnerResult = GenerateWinningResult();
            foreach (var item in BettingList)
            {
                if (WinnerResult.str_WinnerNumber == item.str_ChoiceBet)
                {
                    ResultsList.Add(new BettingResults
                    {
                        str_Client = item.str_ClientId,
                        str_Result = "El resultado a sido " + WinnerResult.str_WinnerNumber + " " + WinnerResult.str_WinnerColor +
                                     " y su elección fue " + item.str_ChoiceBet + ". !Ha ganado en el juego de la ruleta¡",
                        double_Profit = item.double_BetAmount * 5,
                    });
                }
                else
                {
                    if (WinnerResult.str_WinnerColor == item.str_ChoiceBet)
                    {
                        ResultsList.Add(new BettingResults
                        {
                            str_Client = item.str_ClientId,
                            str_Result = "El resultado a sido " + WinnerResult.str_WinnerNumber + " " + WinnerResult.str_WinnerColor +
                                         " y su elección fue " + item.str_ChoiceBet + ". !Ha ganado en el juego de la ruleta¡",
                            double_Profit = item.double_BetAmount * (1.8),
                        });
                    }
                    else
                    {
                        ResultsList.Add(new BettingResults
                        {
                            str_Client = item.str_ClientId,
                            str_Result = "El resultado a sido " + WinnerResult.str_WinnerNumber + " " + WinnerResult.str_WinnerColor +
                                         " y su elección fue " + item.str_ChoiceBet + ". Ha perdido el juego de la ruleta.",
                            double_Profit = -item.double_BetAmount,
                        });
                    }
                }
            }

            return ResultsList;
        }
        #endregion
        #region BetProcess
        public List<Bet> ListBets()
        {
            return _DBProcess.ListAllBets();
        }
        public string CreateBet(Bet bet)
        {
            bet.str_RouletteId = bet.str_RouletteId.Trim();
            bet.str_ChoiceBet = bet.str_ChoiceBet.Trim().ToUpper();
            string ResponseValidates = ValidateRouletteForBet(rouletteId: bet.str_RouletteId);
            if (ResponseValidates != "OK")

                return ResponseValidates;
            ResponseValidates = ValidateDataBet(bet);
            if (ResponseValidates != "OK")

                return ResponseValidates;
            _DBProcess.CreateBet(bet);

            return "Operación aceptada. Apuesta creada de forma exitosa.";
        }
        public string ValidateDataBet(Bet bet)
        {
            if (string.IsNullOrEmpty(bet.str_ClientId))

                return "Debe ingresar un ID de usuario valido para realizar la apuesta.";
            if (string.IsNullOrEmpty(bet.str_ChoiceBet))

                return "Debe elegir un número entre 0 o 36, o escoger ROJO o NEGRO para realizar su apuesta";
            if (bet.double_BetAmount <= 0)

                return "Debe apostar por montos mayores que cero (0)";
            if (bet.double_BetAmount > 10000)

                return "Cada apuesta no puede se mayore a 10.000 dólares en su monto.";
            if (IsNumberValue(value: bet.str_ChoiceBet))
            {
                int ChoiseNumber = Convert.ToInt32(bet.str_ChoiceBet);
                if (ChoiseNumber < 0 || ChoiseNumber > 36)

                    return "Los números disponibles a apostar son del 0 al 36";
            }
            else
            {
                if (bet.str_ChoiceBet != "ROJO" && bet.str_ChoiceBet != "NEGRO")

                    return "Solo puede ingresar las palabras Rojo o Negro para realizar la apuesta";
            }

            return "OK";
        }
        private bool IsNumberValue(string value)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(value);
        }
        #endregion        
    }
}
