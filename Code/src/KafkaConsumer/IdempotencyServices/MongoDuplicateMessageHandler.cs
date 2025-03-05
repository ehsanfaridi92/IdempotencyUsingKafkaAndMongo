using KafkaConsumer.ConfigurationModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace KafkaConsumer.IdempotencyServices;

public class MongoDuplicateMessageHandler(IMongoDatabase database, MongoDbConfiguration mongoDbConfiguration)
    : IDuplicateMessageHandler
{
    public async Task<bool> HasMessageBeenProcessedBefore(string eventId)
    {
        try
        {
            var messageInboxCollection = database.GetCollection<BsonDocument>(mongoDbConfiguration.CollectionName);

            var filter = Builders<BsonDocument>.Filter.Eq(mongoDbConfiguration.FieldName, eventId);

            return await messageInboxCollection.CountDocumentsAsync(filter) > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task MarkMessageAsProcessed(string eventId)
    {
        try
        {
            var messageInboxCollection = database.GetCollection<BsonDocument>(mongoDbConfiguration.CollectionName);

            await messageInboxCollection.InsertOneAsync(new BsonDocument
            {
                { "_id" , ObjectId.GenerateNewId()},
                {mongoDbConfiguration.FieldName,eventId},
                {mongoDbConfiguration.ReceivedDate,DateTime.UtcNow}
            });
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}