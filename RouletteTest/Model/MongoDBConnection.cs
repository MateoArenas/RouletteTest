﻿using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace RouletteTest.Model
{
    public class MongoDBConnection
    {
        #region Roulette
        public List<Roulette> ListRoulettes()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RouletteTest");
            var ProcessDB = database.GetCollection<Roulette>("Roulette");
            return ProcessDB.Find(x => true).ToList();
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
        #endregion
        public void CreateBet(Bet bet)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("RouletteTest");
            var ProcessDB = database.GetCollection<Bet>("Bet");
            ProcessDB.InsertOne(bet);
        }
    }
}
