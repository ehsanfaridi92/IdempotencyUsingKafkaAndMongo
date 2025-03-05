using KafkaConsumer;
using KafkaConsumer.ConfigurationModels;
using KafkaConsumer.IdempotencyServices;
using MongoDB.Driver;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<KafkaConsumerWorker>();

var kafkaConsumerConfiguration = builder.Configuration.GetSection("KafkaConsumerConfiguration").Get<KafkaConsumerConfiguration>();

builder.Services.AddSingleton(kafkaConsumerConfiguration);

var mongoConfiguration = builder.Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();

builder.Services.AddSingleton(mongoConfiguration);

builder.Services.AddSingleton<IMongoDatabase>(CreateMongoDb(mongoConfiguration));

builder.Services.AddSingleton<IDuplicateMessageHandler, MongoDuplicateMessageHandler>();

var host = builder.Build();
host.Run();

static IMongoDatabase CreateMongoDb(MongoDbConfiguration mongoConfiguration)
{
    var client = new MongoClient(mongoConfiguration.Connection);

    return client.GetDatabase(mongoConfiguration.DatabaseName);
}
