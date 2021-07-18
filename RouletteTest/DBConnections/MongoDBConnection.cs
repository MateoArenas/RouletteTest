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
        public List<Roulette> ListRoulettes()
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            return collection.Find(x => true).ToList();
        }
        public Roulette FindRouletteForID(string RouletteID)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            return collection.Find(x => x.Id == RouletteID).FirstOrDefault();
        }
        public bool FindStateRouletteForID(string RouletteID)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette roulette = collection.Find(x => x.Id == RouletteID).FirstOrDefault();
            return roulette.bool_OpeningStatus;
        }
        public void CreateRoulette(Roulette roulette)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            collection.InsertOne(roulette);
        }
        public void OpenRoulette(string RouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette InformationRouletteState = collection.Find(x => x.Id == RouletteId).FirstOrDefault();
            InformationRouletteState.bool_OpeningStatus = true;
            collection.ReplaceOne(x => x.Id == RouletteId, InformationRouletteState);
        }
        public void CloseRoulette(string RouletteId)
        {
            IMongoCollection<Roulette> collection = _mongoDatabase.GetCollection<Roulette>("Roulette");
            Roulette InformationRouletteState = collection.Find(x => x.Id == RouletteId).FirstOrDefault();
            InformationRouletteState.bool_OpeningStatus = false;
            collection.ReplaceOne(x => x.Id == RouletteId, InformationRouletteState);
        }      
        #endregion
        #region Bet
        public void CreateBet(Bet bet)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            collection.InsertOne(bet);
        }
        public void DeleteBetsForRuletteId(string RouletteId)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            collection.DeleteMany(x => x.str_RouletteId == RouletteId);
        }
        public List<Bet> RouletteBettingList(string RouletteId)
        {
            IMongoCollection<Bet> collection = _mongoDatabase.GetCollection<Bet>("Bet");
            return collection.Find(x => x.str_RouletteId == RouletteId).ToList();
        }
        #endregion
    }
}
