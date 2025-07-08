using ChatService.Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using СhatService.DAL.Configuration;
using СhatService.DAL.Documents;
using СhatService.DAL.Interface;

namespace СhatService.DAL.Repository
{
    public class MessageRepository(IMongoDatabase db, IOptions<MongoConfiguration> config) : IMessageRepository
    {
        private readonly IMongoCollection<Message> _collection = db.GetCollection<Message>(config.Value.Collection);

        public async Task<Message> AddAsync(Message message, CancellationToken ct)
        {
            await _collection.InsertOneAsync(message, new(), ct);

            return message;
        }

        public async Task<IEnumerable<Message>> GetAllAsync(Guid realEstateId, string userId, CancellationToken ct)
        {
            var filter = Builders<Message>.Filter.And(
                Builders<Message>.Filter.Eq(m => m.RealEstateId, realEstateId),
                Builders<Message>.Filter.Or(
                    Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                    Builders<Message>.Filter.Eq(m => m.RecieverId, userId)
                )
            );

            var sort = Builders<Message>.Sort.Ascending(m => m.SentAt);

            return await _collection
                .Find(filter)
                .Sort(sort)
                .ToListAsync(ct);
        }

        public async Task<List<DialogPreviewModel>> GetUserDialogsAsync(string userId, CancellationToken ct)
        {
            var filter = Builders<Message>.Filter.Or(
                Builders<Message>.Filter.Eq(m => m.SenderId, userId),
                Builders<Message>.Filter.Eq(m => m.RecieverId, userId));

            var sort = Builders<Message>.Sort
                .Ascending(m => m.RealEstateId)
                .Descending(m => m.SentAt);

            var group = new BsonDocument
            {
                { "_id", "$realEstateId" },
                { "lastMessage", new BsonDocument("$first", "$content") },
                { "lastMessageTime", new BsonDocument("$first", "$sentAt") },
                { "senderId", new BsonDocument("$first", "$senderId") },
                { "recieverId", new BsonDocument("$first", "$recieverId") }
            };

            var project = new BsonDocument
            {
                { "realEstateId", "$_id" },
                { "lastMessage", 1 },
                { "lastMessageTime", 1 },
                { "interlocutorId", new BsonDocument("$cond", new BsonArray
                    {
                        new BsonDocument("$eq", new BsonArray { "$senderId", userId }),
                        "$recieverId",
                        "$senderId"
                    })
                }
            };

            var pipeline = _collection.Aggregate()
                .Match(filter)
                .Sort(sort)
                .Group(
                    key => key.RealEstateId,
                    g => new
                    {
                        RealEstateId = g.Key,
                        LastMessage = g.First().Content,
                        LastMessageTime = g.First().SentAt,
                        SenderId = g.First().SenderId,
                        ReceiverId = g.First().RecieverId
                    })
                .Project(g => new DialogPreviewModel
                {
                    RealEstateId = g.RealEstateId,
                    LastMessage = g.LastMessage,
                    LastMessageTime = g.LastMessageTime,
                    InterlocutorId = g.SenderId == userId ? g.ReceiverId : g.SenderId
                });

            return await pipeline.ToListAsync(ct);
        }
    }
}
