using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace СhatService.DAL.Documents
{
    public class Message
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("realEstateId")]
        [BsonRepresentation(BsonType.String)]
        public Guid RealEstateId { get; set; }

        [BsonElement("senderId")]
        public string SenderId { get; set; } = null!;

        [BsonElement("recieverId")]
        public string RecieverId { get; set; } = null!;

        [BsonElement("content")]
        public string Content { get; set; } = null!;

        [BsonElement("sentAt")]
        public DateTime SentAt { get; set; }
    }
}
