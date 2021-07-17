using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RouletteTest.Model
{
    public class Roulette
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("openingstatus")]
        public bool bool_OpeningStatus { get; set; }
    }
}
