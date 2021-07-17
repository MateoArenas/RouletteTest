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
            var roulette = new Roulette() {Id= "60f2fa152f687213605b4cd9", str_Name = "Prueba Ruleta", bool_OpeningStatus = false };
            ProcessDB.InsertOne(roulette);
        }
    }
}
