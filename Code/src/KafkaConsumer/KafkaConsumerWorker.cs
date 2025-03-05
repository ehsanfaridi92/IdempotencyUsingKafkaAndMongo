using Confluent.Kafka;
using KafkaConsumer.ConfigurationModels;
using KafkaConsumer.IdempotencyServices;
using KafkaConsumer.Services;

namespace KafkaConsumer
{
    public class KafkaConsumerWorker(IDuplicateMessageHandler duplicateMessageHandler, KafkaConsumerConfiguration kafkaConsumerConfiguration)
        : KafkaIdempotentConsumer(duplicateMessageHandler, kafkaConsumerConfiguration)
    {

        protected override async Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult)
        {
            Console.WriteLine($"message {consumeResult.Message.Value} consumed");

            await Task.CompletedTask;
        }
    }
}
