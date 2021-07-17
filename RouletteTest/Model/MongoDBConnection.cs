using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteTest.Model
{
    public class MongoDBConnection
    {
        public void ConnectMongoDB()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RouletteTest");
            var ProcessDB = database.GetCollection<Roulette>("Roulette");
        }
        public void CreateRoulette(Roulette roulette)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RouletteTest");
            var ProcessDB = database.GetCollection<Roulette>("Roulette");        
            ProcessDB.InsertOne(roulette);
        }
        public void OpenRoulette(string id)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RouletteTest");
            var ProcessDB = database.GetCollection<Roulette>("Roulette");
            var NewRouletteState = new Roulette() { Id = id, bool_OpeningStatus = true };
            ProcessDB.ReplaceOne(x => x.Id == id, NewRouletteState);
        }
    }
}
