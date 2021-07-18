using MongoDB.Driver;
using RouletteTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace RouletteTest.DBConnections
{
    public class MongoDBConnection
    {
        private MongoClient _mongoClient;
        private IMongoDatabase _mongoDatabase;
        public MongoDBConnection()
        {
            _mongoClient = new MongoClient("mongodb://localhost:27017");
            _mongoDatabase = _mongoClient.GetDatabase("RouletteTest"); ;
        }
        #region Roulette
        public List<Roulette> ListAllRoulettes()
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");

            return collection.Find(x => true).ToList();
        }
        public Roulette FindRouletteForID(string rouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");

            return collection.Find(x => x.Id == rouletteId).FirstOrDefault();
        }
        public bool FindStateRouletteForID(string rouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette roulette = collection.Find(x => x.Id == rouletteId).FirstOrDefault();

            return roulette.bool_OpeningStatus;
        }
        public void CreateRoulette(Roulette roulette)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            collection.InsertOne(roulette);
        }
        public void OpenRoulette(string rouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette InformationRouletteState = collection.Find(x => x.Id == rouletteId).FirstOrDefault();
            InformationRouletteState.bool_OpeningStatus = true;
            collection.ReplaceOne(x => x.Id == rouletteId, InformationRouletteState);
        }
        public void CloseRoulette(string rouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette InformationRouletteState = collection.Find(x => x.Id == rouletteId).FirstOrDefault();
            InformationRouletteState.bool_OpeningStatus = false;
            collection.ReplaceOne(x => x.Id == rouletteId, InformationRouletteState);
        }
        #endregion
        #region Bet
        public List<Bet> ListAllBets()
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            return collection.Find(x => true).ToList();
        }
        public void CreateBet(Bet bet)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            collection.InsertOne(bet);
        }
        public void DeleteBetsForRuletteId(string rouletteId)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            collection.DeleteMany(x => x.str_RouletteId == rouletteId);
        }
        public List<Bet> BettingListForRouletteId(string rouletteId)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");

            return collection.Find(x => x.str_RouletteId == rouletteId).ToList();
        }
        #endregion
    }
}
