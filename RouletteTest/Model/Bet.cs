using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RouletteTest.Model
{
    public class Bet
    {
        [JsonIgnore]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonElement("clientid")]
        public string str_ClientId { get; set; }
        [BsonElement("roulette")]
        public string str_Roulette { get; set; }
        [BsonElement("betamount")]
        public double double_BetAmount { get; set; }
        [BsonElement("choicebet")]
        public string str_ChoiceBet { get; set; }
    }
}
