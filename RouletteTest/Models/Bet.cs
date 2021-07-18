using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace RouletteTest.Models
{
    public class Bet
    {
        [JsonIgnore]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("clientid")]
        public string str_ClientId { get; set; }
        [BsonElement("rouletteid")]
        public string str_RouletteId { get; set; }
        [BsonElement("betamount")]
        public double double_BetAmount { get; set; }
        [BsonElement("choicebet")]
        public string str_ChoiceBet { get; set; }
    }
}
