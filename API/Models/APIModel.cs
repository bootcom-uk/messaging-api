using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class APIModel
    {

        [BsonId]
        public ObjectId Id { get; set; }

        public string APIKey { get; set; }

        public DateTime DateAdded { get; set; }

    }
}
