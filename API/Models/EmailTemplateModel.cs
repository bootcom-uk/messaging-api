using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class EmailTemplateModel
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("content")]
        public string EmailBody { get; set; }

        [BsonElement("isHtml")]
        public bool IsHtmlEmail { get; set; }

        [BsonElement("subject")]
        public required string EmailSubject { get; set; }

    }
}
